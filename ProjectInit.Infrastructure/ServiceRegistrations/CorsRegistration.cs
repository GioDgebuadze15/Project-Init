using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class CorsRegistration
{
    //Todo: move this into constants file
    private const string VueClientOrigin = "http://localhost:8080";
    private const string AllowVueClientName = "AllowVueClient";
    private const string AllowAllClientsName = "AllowAllClients";
    
    public static IServiceCollection AddApiCors(this IServiceCollection @this, IHostEnvironment env)
    {
        @this.AddCors(corsOptions =>
        {
            if (env.IsProduction())
            {
                corsOptions.AddPolicy(AllowVueClientName, policy =>
                {
                    policy.WithOrigins(VueClientOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            }
            else if (env.IsDevelopment())
            {
                corsOptions.AddPolicy(AllowAllClientsName, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }
        });


        return @this;
    }

    public static IApplicationBuilder UseApiCors(this IApplicationBuilder @this, IHostEnvironment env)
    {
        var policyName = env.IsProduction()
            ? nameof(AllowVueClientName)
            : env.IsDevelopment()
                ? nameof(AllowAllClientsName)
                : "";
        
        @this.UseCors(policyName);

        return @this;
    }
}