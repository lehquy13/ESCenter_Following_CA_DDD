using Matt.AutoDI;

namespace Matt.SharedKernel.Application.Contracts.Interfaces;

public interface IAsyncQueryableExecutor : IScoped
{
    Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, bool isTracking = true,
        CancellationToken cancellationToken = default) where T : class;
    
    Task<List<T>> ToListAsSplitAsync<T>(IQueryable<T> queryable, bool isTracking = true,
        CancellationToken cancellationToken = default) where T : class;

    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, bool isTracking = true,
        CancellationToken cancellationToken = default) where T : class;

    Task<T?> SingleOrDefault<T>(IQueryable<T> queryable, bool isTracking = true,
        CancellationToken cancellationToken = default) where T : class;

    Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        where T : class;

    Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
}