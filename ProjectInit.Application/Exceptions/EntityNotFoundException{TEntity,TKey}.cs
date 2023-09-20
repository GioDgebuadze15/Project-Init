namespace ProjectInit.Application.Exceptions;

public class EntityNotFoundException<TEntity, TKey> : EntityNotFoundException
    where TEntity : notnull
    where TKey : notnull
{
    //TODO: entityId must not be null here, so add some checking somehow
    public EntityNotFoundException(TKey entityId)
        : base(typeof(TEntity).Name, entityId.ToString())
    {
    }
}