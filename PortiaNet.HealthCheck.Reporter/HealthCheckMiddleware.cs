using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using System.Text.Json;

namespace PortiaNet.HealthCheck.Reporter
{
    /// <summary>
    /// This middleware tracks the requests with the full client information, request duration, and success status.
    /// It should be added after the global exception handling, Authentication, and Authorization middlewares (if exist)
    /// </summary>
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        public HealthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IEnumerable<IHealthCheckReportService> implementations)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<HealthCheckIgnoreAttribute>();
            if (attribute != null)
            {
                await _next.Invoke(context);
                return;
            }


            string queryString;
            try
            {
                queryString = JsonSerializer.Serialize(context.Request.Query);
            }
            catch
            {
                queryString = context.Request.QueryString.Value;
            }

            var userAgent = context.Request.Headers.ContainsKey("User-Agent") ?
                context.Request.Headers["User-Agent"].FirstOrDefault() : string.Empty;

            var startTime = DateTime.Now;
            var hasError = false;

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                if (ex is not IHealthCheckKnownException)
                    hasError = true;

                Debugger.Log(100, "HealthCheck Exception", ex.Message);
                Debugger.Log(100, "HealthCheck Exception", Environment.NewLine);
                Debugger.Log(100, "HealthCheck Exception", ex.StackTrace);
                Debugger.Log(100, "HealthCheck Exception", Environment.NewLine);
                throw;
            }
            finally
            {
                var duration = DateTime.Now - startTime;
                var requestDetail = new RequestDetail
                {
                    Duration = duration.TotalMilliseconds,
                    HadError = hasError,
                    Host = context.Request.Host.Value,
                    IpAddress = context.Connection.RemoteIpAddress.ToString(),
                    Method = context.Request.Method,
                    Path = context.Request.Path,
                    QueryString = queryString,
                    UserAgent = userAgent,
                    Username = context.User?.Identity?.Name,
                    EventDateTime = DateTime.UtcNow
                };

                try
                {
                    foreach (var impl in implementations)
                        await impl.SaveAPICallInformationAsync(requestDetail);
                }
                catch(Exception ex)
                {
                    Debugger.Log(100, "HealthCheck Save Call Exception", ex.Message);
                    Debugger.Log(100, "HealthCheck Save Call Exception", Environment.NewLine);
                    Debugger.Log(100, "HealthCheck Save Call Exception", ex.StackTrace);
                    Debugger.Log(100, "HealthCheck Save Call Exception", Environment.NewLine);
                    throw;
                }
            }
        }
    }
}
