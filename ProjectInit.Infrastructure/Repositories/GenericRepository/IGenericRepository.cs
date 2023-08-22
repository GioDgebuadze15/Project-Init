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

    IQueryable<TEntity> GetById(int id);


    Task<TEntity> GetByIdAsync(int id);

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