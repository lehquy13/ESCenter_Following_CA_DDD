using ESCenter.Persistence.Entity_Framework_Core;
using Matt.AutoDI;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using Matt.SharedKernel.Domain.Primitives;
using Matt.SharedKernel.Domain.Primitives.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class RepositoryImpl<TEntity, TId>(
    AppDbContext appDbContext,
    IAppLogger<RepositoryImpl<TEntity, TId>> logger)
    : ReadOnlyRepositoryImpl<TEntity, TId>(appDbContext, logger), IRepository<TEntity, TId>,
        IOpenGenericService<IRepository<TEntity, TId>>
    where TEntity : Entity<TId>, IAggregateRoot<TId>
    where TId : notnull
{
    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await AppDbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in InsertAsync", ex.Message);
        }
    }

    public async Task InsertManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            await AppDbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in InsertManyAsync", ex.Message);
        }
    }
    
    public async Task<TEntity> UpdateAsync(TEntity entity,  CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        AppDbContext.Attach(entity);

        var updatedEntity = AppDbContext.Update(entity).Entity;

        return updatedEntity;
    }

    public async Task UpdateManyAsync(IEnumerable<TEntity> entities,  CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var entityArray = entities.ToArray();
        
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

        AppDbContext.Set<TEntity>().UpdateRange(entityArray);
    }


    public async Task<bool> RemoveAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            var deleteRecord = await AppDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);

            if (deleteRecord == null) return false;

            AppDbContext.Set<TEntity>().Remove(deleteRecord);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return new bool();
        }
    }

    public async Task RemoveManyAsync(IEnumerable<TId> ids, 
        CancellationToken cancellationToken = default(CancellationToken))
    {
        await Task.CompletedTask;
        try
        {
            var deleteRecords = AppDbContext.Set<TEntity>().Where(x => ids.Contains(x.Id));

            AppDbContext.Set<TEntity>().RemoveRange(deleteRecords);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
        }
    }

    public async Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in FindAsync", ex.Message);
            return null;
        }
    }
}