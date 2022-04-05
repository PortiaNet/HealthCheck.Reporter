namespace PortiaNet.HealthCheck.Reporter
{
    /// <summary>
    /// This interface is getting used to handle the known exception in the global exception handling. 
    /// If any know exception gets thrown by the API, the health check middleware won't mark the request with the <b>HadError</b> flag.
    /// </summary>
    public interface IHealthCheckKnownException
    {
    }
}
