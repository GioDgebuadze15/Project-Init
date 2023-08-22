using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class LoggerRegistration
{
    public static IApplicationBuilder UseLoggerFactory(this IApplicationBuilder @this, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddFile("Logs/api-{Date}.log");
        return @this;
    }
}