using ProjectInit.Domain.Handlers.NotificationHandler;
using Wolverine;

namespace ProjectInit.Shared.Extensions;

public static class WolverineExtensions
{
    public static async ValueTask DispatchDomainEvent(this IMessageBus bus, INotification @event)
        => await bus.PublishAsync(@event);
}