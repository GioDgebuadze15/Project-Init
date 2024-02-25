using Microsoft.Extensions.DependencyInjection;
using ProjectInit.Infrastructure.Repositories.GenericRepository;
using ProjectInit.Infrastructure.Services.File;
using Transmogrify;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class ServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        @this.AddScoped<ITranslator, Translator>();
        @this.AddScoped<IFileService, FileService>();

        return @this;
    }
}