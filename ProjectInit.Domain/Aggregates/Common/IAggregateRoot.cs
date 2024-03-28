using ProjectInit.Domain.Handlers.NotificationHandler;

namespace ProjectInit.Domain.Aggregates.Common;

public interface IAggregateRoot
{
    public IReadOnlyCollection<INotification> DomainEvents { get; }
    public void Raise(INotification @event);
    public void ClearDomainEvents();
}