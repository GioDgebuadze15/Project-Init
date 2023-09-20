namespace ProjectInit.Infrastructure.Repositories.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    #region Create

    Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = new());

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = new());

    Task AddRange(TEntity[] entities, CancellationToken cancellationToken = new());

    Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken = new());

    #endregion

    #region Read

    IQueryable<TEntity> GetAll();

    Task<IEnumerable<TEntity>> GetAllAsync();

    IQueryable<TEntity> GetById<TKey>(TKey id) where TKey : notnull;

    Task<TEntity> GetByIdAsync<TKey>(TKey id) where TKey : notnull;

    #endregion

    #region Update

    Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = new());

    Task UpdateRange(TEntity[] entities, CancellationToken cancellationToken = new());

    #endregion

    #region Delete

    Task Delete(TEntity entity, CancellationToken cancellationToken = new());
    Task DeleteRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new());

    #endregion
}