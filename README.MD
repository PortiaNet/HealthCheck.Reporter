<img src="https://user-images.githubusercontent.com/11803924/159634426-45568864-0568-441c-9d0b-a28756f42f9f.png" width="400">


[![Nuget](https://img.shields.io/nuget/v/Portia.Net.HealthCheck?label=NuGet)](https://www.nuget.org/packages/Portia.Net.HealthCheck/)

### What is Portia.Net.HealthCheck?

![](https://github.com/AminEsmaeily/Portia.Net.HealthCheck/blob/master/Assets/logo-16.png) **Portia.Net.HealthCheck** is a middleware for .Net web applications (WebAPI and MVC) which collects all required information about the income requests on the server, then delivers them to a service to be saved or analyzed.

The following project has the core functionality of the health check which only collects the information and delivers them to a registered service with the type of [IHealthCheckReportService](https://github.com/AminEsmaeily/Portia.Net.HealthCheck/blob/master/Portia.Net.HealthCheck.Core/IHealthCheckReportService.cs).

The information which this middleware provides are as follows:

- Client IP Address
- User Identity (if using authentication middleware)
- Host Address which has received the request (Usefull for the load balancing)
- Request Method Type (Get, Put, Push, Patch, Delete)
- API Path which has been called
- Query String
- Full User Agent Information
- Duration time which the API call took
- Success Result of the API (To check if API throws any unknown or unhandled exception)

<img src="https://user-images.githubusercontent.com/11803924/159649606-9b56f148-f28f-42f7-be1b-e0d15c41d1a7.png" width="600">


### Installation

You can install this tool from [Nuget](https://www.nuget.org/packages/Portia.Net.HealthCheck/) using Package Manager Console:

```
PM> Install-Package Portia.Net.HealthCheck
```

### How do I get started?

1. You need a service in your repository to receive the reports from the middleware, then create a simple one and inherit it from [IHealthCheckReportService](https://github.com/AminEsmaeily/Portia.Net.HealthCheck/blob/master/Portia.Net.HealthCheck.Core/IHealthCheckReportService.cs).

``` C#
public class HealthCheckReportServiceImpl : IHealthCheckReportService
{
    public Task SaveAPICallInformationAsync(RequestDetail requestDetail)
    {
        // Using this method, you can save all collected information in a cloud, local, or third-party database.
        // It is highly recommended to save the tracked information in another database than the main one due to decrease the performance issue
    }
}
```

2. The service should be added to the DI in the Configuration section.

``` C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IHealthCheckReportService, HealthCheckReportServiceImpl>();
```

3. Add the middleware to the pipeline. Keep in mind to add it after `Authentication` middleware (if any, otherwise the logged-in user's identity will be always empty). It is recommended to put this middleware as the last one to have accurate API duration information.

``` C#
var app = builder.Build();
...
app.UseAuthentication();
...
app.UseHealthCheck();
...
```


### How does it work?
When the HealthCheck middleware receives the request, it checks the attributes of the target API, and if it had the `HealthCheckIgnoreAttribute` attribute then the request won't be tracked.

In the next step, the middleware forwards the request to the next ones in the pipeline. This middleware catches all exceptions which may get thrown by the next middlewares, and if the thrown exception wasn't a subtype of `IHealthCheckKnownException`, then it will mark the request track entity with an error. Then rethrows the exception to be handled by the previous middlewares (e.g. global exception handler). But before going back to the previous middleware, it sends all gathered information to the `SaveAPICallInformationAsync` method of the injected `IHealthCheckReportService` service.


![chart drawio](https://user-images.githubusercontent.com/11803924/159649428-0ebc6a16-a0f7-45c4-b6c3-c741e950f39f.png)

