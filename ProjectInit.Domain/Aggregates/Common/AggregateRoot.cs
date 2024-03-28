using System.Text.Json.Serialization;
using ProjectInit.Domain.Entities.Common;
using ProjectInit.Domain.Handlers.NotificationHandler;

namespace ProjectInit.Domain.Aggregates.Common;

public abstract class AggregateRoot<TKey> : BaseEntity<TKey>, IAggregateRoot
    where TKey : notnull
{
    private readonly List<INotification> _domainEvents = [];
    
    [JsonIgnore]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
    public void Raise(INotification @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}