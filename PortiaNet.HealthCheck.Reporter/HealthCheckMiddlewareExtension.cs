using Microsoft.AspNetCore.Builder;

namespace PortiaNet.HealthCheck.Reporter
{
    public static class HealthCheckMiddlewareExtension
    {
        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app)
        {
            app.UseMiddleware<HealthCheckMiddleware>();
            return app;
        }
    }
}
