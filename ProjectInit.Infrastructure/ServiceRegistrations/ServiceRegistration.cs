using Microsoft.Extensions.DependencyInjection;
using Transmogrify;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class ServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection @this)
    {
        @this.AddScoped<ITranslator, Translator>();
        return @this;
    }
}