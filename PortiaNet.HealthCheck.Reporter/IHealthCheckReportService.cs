namespace PortiaNet.HealthCheck.Reporter
{
    public interface IHealthCheckReportService
    {
        /// <summary>
        /// Gets called by the health-check middleware when the api process completed.
        /// </summary>
        /// <param name="requestDetail">All available details of the request will be passed to the method.</param>
        /// <returns></returns>
        Task SaveAPICallInformationAsync(RequestDetail requestDetail); 
    }
}
