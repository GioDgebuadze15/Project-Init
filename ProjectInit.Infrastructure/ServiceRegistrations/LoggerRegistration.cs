using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class LoggerRegistration
{
    public static ILoggerFactory AddLoggerFactory(this IServiceCollection @this, IHostEnvironment env)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug && env.IsDevelopment())
                .WriteTo.Console())
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(_ => env.IsProduction())
                .WriteTo.File($"Logs/api-{DateTime.Now:yyyy-MM-dd}.log"))
            .CreateLogger();

        return LoggerFactory.Create(builder => { builder.AddSerilog(Log.Logger); });
    }
}