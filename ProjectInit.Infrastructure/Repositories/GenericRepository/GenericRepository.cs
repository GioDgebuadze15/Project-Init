using Microsoft.EntityFrameworkCore;
using ProjectInit.Application.Exceptions;
using ProjectInit.Domain;
using ProjectInit.Domain.Entities.Common;

namespace ProjectInit.Infrastructure.Repositories.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly AppDbContext _ctx;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = ctx.Set<TEntity>();
    }

    #region Create

    public async Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = new())
    {
        _dbSet.Add(entity);
        _ctx.Entry(entity).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = new())
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        _ctx.Entry(entity).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRange(TEntity[] entities, CancellationToken cancellationToken = new())
    {
        _dbSet.AddRange(entities);
        _ctx.Entry(entities).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken = new())
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        _ctx.Entry(entities).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Read

    public IQueryable<TEntity> GetAll()
        => _dbSet.AsQueryable();

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public IQueryable<TEntity> GetById<TKey>(TKey id)
        where TKey : notnull
    {
        var entity = _dbSet.Find(id);
        return entity is null
            ? throw new EntityNotFoundException<TEntity,TKey>(id)
            : _dbSet.Where(x => x.Equals(entity)).AsQueryable();
    }


    public async Task<TEntity> GetByIdAsync<TKey>(TKey id)
        where TKey : notnull
    {
        var entity = await _dbSet.FindAsync(id);
        return entity is null
            ? throw new EntityNotFoundException<TEntity,TKey>(id)
            : await _dbSet.Where(x => x.Equals(entity)).FirstAsync();
    }

    #endregion

    #region Update

    public async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = new())
    {
        _dbSet.Attach(entity);
        _ctx.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateRange(TEntity[] entities, CancellationToken cancellationToken = new())
    {
        _dbSet.UpdateRange(entities);
        _ctx.Entry(entities).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Delete

    public async Task Delete(TEntity entity, CancellationToken cancellationToken = new())
        =>
            await SoftDelete(entity, cancellationToken);

    public async Task DeleteRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new())
        => await SoftDeleteRange(entities, cancellationToken);

    #endregion

    #region PrivateMethods

    private async Task SoftDelete(TEntity entity, CancellationToken cancellationToken = new())
    {
        if (entity is not BaseEntity baseEntity)
            throw new ArgumentException("Entity must derive from BaseEntity");

        //Todo: assure this logic is correct here
        baseEntity.Deleted = true;
        _ctx.Attach(baseEntity);
        _ctx.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
        throw new ArgumentException("Entity must derive from BaseEntity");
    }

    private async Task SoftDeleteRange(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = new())
    {
        foreach (var entity in entities)
        {
            if (entity is not BaseEntity baseEntity)
                throw new ArgumentException("Entity must derive from BaseEntity");

            //Todo: assure this logic is correct here
            baseEntity.Deleted = true;
            _ctx.Attach(baseEntity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        await SaveChangesAsync(cancellationToken);
        throw new ArgumentException("Entity must derive from BaseEntity");
    }

    #endregion

    #region ProtectedMethods

    protected virtual async Task SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await _ctx.SaveChangesAsync(cancellationToken);
    }

    #endregion
}