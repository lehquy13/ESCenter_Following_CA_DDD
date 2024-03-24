using ESCenter.Persistence.EntityFrameworkCore;
using Matt.AutoDI;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using Matt.SharedKernel.Domain.Primitives;
using Matt.SharedKernel.Domain.Primitives.Abstractions;
using Matt.SharedKernel.Domain.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class ReadOnlyRepositoryImpl<TEntity, TId>(
    AppDbContext appDbContext,
    IAppLogger<ReadOnlyRepositoryImpl<TEntity, TId>> logger)
    : IReadOnlyRepository<TEntity, TId>, IOpenGenericService<IReadOnlyRepository<TEntity, TId>>
    where TEntity : Entity<TId>, IEntity<TId>
    where TId : notnull
{
    protected readonly AppDbContext AppDbContext = appDbContext;
    protected readonly IAppLogger<ReadOnlyRepositoryImpl<TEntity, TId>> Logger = logger;
    protected readonly string ErrorMessage = "{Message} with exception: {Ex}";

    public async Task<List<TEntity>> GetListAsync(IPaginatedGetListSpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec);

        return await specificationResult.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.Set<TEntity>().CountAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return 0;
        }
    }

    public async Task<long> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec, true);

        return await specificationResult.CountAsync(cancellationToken);
    }

    // Consider to add NoTracking
    public IQueryable<TEntity> GetAll()
    {
        try
        {
            return AppDbContext.Set<TEntity>().AsQueryable<TEntity>();
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in GetAll", ex.Message);
            throw;
        }
    }

    public async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.Set<TEntity>().FindAsync([id], cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in GetAsync", ex.Message);
            return null;
        }
    }

    protected static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification, bool isForCount = false)

    {
        var query = inputQuery;

        if (specification.Criteria is not null) query = query.Where(specification.Criteria);

        query = specification
            .IncludeExpressions
            .Aggregate(query, (current, include) => current.Include(include));

        //Handle then include
        query = specification
            .IncludeStrings
            .Aggregate(query, (current, include) => current.Include(include));

        if (specification.IsPagingEnabled && !isForCount)
            query = query.Skip(specification.Skip)
                .Take(specification.Take);

        return query;
    }

    public async Task<TEntity?> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec);

        return await specificationResult.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in GetListAsync", ex.Message);
            return new List<TEntity>();
        }
    }

    public async Task<List<TEntity>> GetListAsync(IGetListSpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec);

        return await specificationResult.AsNoTracking().ToListAsync(cancellationToken);
    }
}