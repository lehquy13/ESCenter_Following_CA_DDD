using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class AsyncQueryableExecutor(AppDbContext dbContext) : IAsyncQueryableExecutor
{
    public async Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, bool isTracking,
        CancellationToken cancellationToken = default) where T : class
    {
        if (!isTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, bool isTracking,
        CancellationToken cancellationToken = default) where T : class
    {
        if (!isTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> SingleOrDefault<T>(IQueryable<T> queryable, bool isTracking,
        CancellationToken cancellationToken = default) where T : class
    {
        if (!isTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        return await queryable.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.LongCountAsync(cancellationToken);
    }

    public async Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.CountAsync(cancellationToken);
    }
}