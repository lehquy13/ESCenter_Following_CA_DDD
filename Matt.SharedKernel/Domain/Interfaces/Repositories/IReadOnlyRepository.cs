using Matt.AutoDI;
using Matt.SharedKernel.Domain.Primitives.Abstractions;
using Matt.SharedKernel.Domain.Specifications.Interfaces;

namespace Matt.SharedKernel.Domain.Interfaces.Repositories;

//TODO: Consider to change IEntity to IAggregateRoot
public interface IReadOnlyRepository<TEntity, TId> : IScoped
    where TEntity : class, IEntity<TId> where TId : notnull
{
    /// <summary>
    /// Get all the record of tables and able to query with linq due to the Queryable return
    /// Consider to remove this method
    /// </summary>
    IQueryable<TEntity> GetAll();
    
    /// <summary>
    /// Get the entity by the its Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get the entity by the specification
    /// </summary>
    /// <param name="spec"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all the records of the table and return the list of entities
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the list of entities by the specification
    /// </summary>
    /// <param name="spec"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(IGetListSpecification<TEntity> spec,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spec"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(IPaginatedGetListSpecification<TEntity> spec,
        CancellationToken cancellationToken = default);

    Task<long> CountAsync(CancellationToken cancellationToken = default);

    Task<long> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
}