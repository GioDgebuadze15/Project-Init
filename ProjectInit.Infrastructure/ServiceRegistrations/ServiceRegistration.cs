using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using ProjectInit.Domain.Handlers.NotificationHandler;
using ProjectInit.Infrastructure.Services.File;
using ProjectInit.Infrastructure.Services.Notification;
using ProjectInit.Persistence.Repositories.GenericRepository;
using Transmogrify;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class ServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        @this.AddScoped<ITranslator, Translator>();
        @this.AddScoped<IFileService, FileService>();

        @this.AddSingleton(Channel.CreateUnbounded<INotification>());
        @this.AddHostedService<NotificationDispatcher>();

        return @this;
    }
}