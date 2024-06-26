﻿using Microsoft.EntityFrameworkCore;
using ProjectInit.Domain.Entities.Common;
using ProjectInit.Shared.Constants;
using ProjectInit.Shared.Exceptions;

namespace ProjectInit.Persistence.Repositories.GenericRepository;

public class GenericRepository<TEntity>(AppDbContext ctx) : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = ctx.Set<TEntity>();

    #region Create

    public async Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Add(entity);
        ctx.Entry(entity).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        ctx.Entry(entity).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRange(TEntity[] entities, CancellationToken cancellationToken = default)
    {
        _dbSet.AddRange(entities);
        ctx.Entry(entities).State = EntityState.Added;
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        ctx.Entry(entities).State = EntityState.Added;
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
            ? throw new EntityNotFoundException<TEntity, TKey>(id)
            : _dbSet.Where(x => x.Equals(entity)).AsQueryable();
    }


    public async Task<TEntity> GetByIdAsync<TKey>(TKey id)
        where TKey : notnull
    {
        var entity = await _dbSet.FindAsync(id);
        return entity is null
            ? throw new EntityNotFoundException<TEntity, TKey>(id)
            : await _dbSet.Where(x => x.Equals(entity)).FirstAsync();
    }

    #endregion

    #region Update

    public async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Attach(entity);
        ctx.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateRange(TEntity[] entities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        ctx.Entry(entities).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Delete

    public async Task HardDelete(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDelete(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is not BaseEntity baseEntity)
            throw new ArgumentException(ExceptionConstants.BaseEntityException);

        baseEntity.SoftDelete();
        ctx.Attach(baseEntity);
        ctx.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
    }

    public async Task HardDeleteRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteRange(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (entity is not BaseEntity baseEntity)
                throw new ArgumentException(ExceptionConstants.BaseEntityException);

            baseEntity.SoftDelete();
            ctx.Attach(baseEntity);
            ctx.Entry(entity).State = EntityState.Modified;
        }

        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region ProtectedMethods

    protected virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await ctx.SaveChangesAsync(cancellationToken);

    #endregion
}