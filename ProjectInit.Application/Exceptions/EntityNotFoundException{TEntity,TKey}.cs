namespace ProjectInit.Application.Exceptions;

public class EntityNotFoundException<TEntity, TKey>(TKey entityId)
    : EntityNotFoundException(typeof(TEntity).Name, entityId.ToString())
    where TEntity : notnull
    where TKey : notnull;