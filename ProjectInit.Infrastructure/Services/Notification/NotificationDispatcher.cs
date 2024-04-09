using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectInit.Domain.Handlers.NotificationHandler;
using ProjectInit.Shared.Extensions;
using Wolverine;

namespace ProjectInit.Infrastructure.Services.Notification;

public class NotificationDispatcher(
    Channel<INotification, INotification> channel,
    IServiceProvider serviceProvider
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!channel.Reader.Completion.IsCompleted)
        {
            var notification = await channel.Reader.ReadAsync(stoppingToken);

            using var scope = serviceProvider.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            await messageBus.DispatchDomainEvent(notification);
        }
    }
}