using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectInit.Application.Constants;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class CorsRegistration
{
    public static IServiceCollection AddApiCors(this IServiceCollection @this, IHostEnvironment env)
    {
        @this.AddCors(corsOptions =>
        {
            if (env.IsProduction())
            {
                corsOptions.AddPolicy(ClientConstants.AllowVueClientName, policy =>
                {
                    policy.WithOrigins(ClientConstants.VueClientOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            }
            else if (env.IsDevelopment())
            {
                corsOptions.AddPolicy(ClientConstants.AllowAllClientsName, policy =>
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
            ? nameof(ClientConstants.AllowVueClientName)
            : env.IsDevelopment()
                ? nameof(ClientConstants.AllowAllClientsName)
                : ClientConstants.DefaultPolicyName;

        @this.UseCors(policyName);

        return @this;
    }
}