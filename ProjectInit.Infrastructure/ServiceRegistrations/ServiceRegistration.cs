using Microsoft.Extensions.DependencyInjection;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class ServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection @this)
    {
        return @this;
    }
}