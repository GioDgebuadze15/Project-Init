namespace ProjectInit.Application.Exceptions;

public abstract class EntityNotFoundException : KeyNotFoundException
{
    public string EntityType { get; private set; }
    public string EntityId { get; private set; }

    protected EntityNotFoundException(string entityType, string entityId)
        : base($"{entityType}-{entityId}")
    {
        EntityType = entityType;
        EntityId = entityId;
    }
}