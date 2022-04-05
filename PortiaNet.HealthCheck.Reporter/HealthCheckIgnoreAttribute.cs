namespace PortiaNet.HealthCheck.Reporter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HealthCheckIgnoreAttribute : Attribute
    {
    }
}
