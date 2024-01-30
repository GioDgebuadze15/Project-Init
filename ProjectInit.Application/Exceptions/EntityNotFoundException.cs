namespace ProjectInit.Application.Exceptions;

public abstract class EntityNotFoundException(string entityType, string entityId)
    : KeyNotFoundException($"{entityType} with Id: {entityId} was not found.")
{
    public string EntityType { get; private init; } = entityType;
    public string EntityId { get; private init; } = entityId;
}