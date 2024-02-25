namespace ProjectInit.Shared.Exceptions;

public class EntityNotFoundException<TEntity, TKey>(TKey entityId)
    : EntityNotFoundException(typeof(TEntity).Name, entityId.ToString() ?? string.Empty)
    where TEntity : notnull
    where TKey : notnull;