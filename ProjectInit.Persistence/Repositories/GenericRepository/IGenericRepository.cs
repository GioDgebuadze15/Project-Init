namespace ProjectInit.Persistence.Repositories.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    #region Create

    Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRange(TEntity[] entities, CancellationToken cancellationToken = default);

    Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default);

    #endregion

    #region Read

    IQueryable<TEntity> GetAll();

    Task<IEnumerable<TEntity>> GetAllAsync();

    IQueryable<TEntity> GetById<TKey>(TKey id) where TKey : notnull;

    Task<TEntity> GetByIdAsync<TKey>(TKey id) where TKey : notnull;

    #endregion

    #region Update

    Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRange(TEntity[] entities, CancellationToken cancellationToken = default);

    #endregion

    #region Delete

    Task HardDelete(TEntity entity, CancellationToken cancellationToken = default);
    Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default);
    Task HardDeleteRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task SoftDeleteRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion
}