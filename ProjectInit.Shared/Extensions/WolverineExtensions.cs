using System.Collections.Immutable;
using ProjectInit.Domain.Handlers.NotificationHandler;
using Wolverine;

namespace ProjectInit.Shared.Extensions;

public static class WolverineExtensions
{
    public static ValueTask DispatchDomainEvents(this IMessageBus bus, ImmutableArray<INotification> events)
        => bus.PublishAsync(events);
}