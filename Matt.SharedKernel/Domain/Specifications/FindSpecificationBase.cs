using Matt.SharedKernel.Domain.Specifications.Interfaces;

namespace Matt.SharedKernel.Domain.Specifications;

/// <summary>
/// Find a TEntity by Id
/// </summary>
/// <param name="id"></param>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class FindSpecificationBase<TEntity, TId>(TId id)
    : SpecificationBase<TEntity>,
        IFindSpecification<TEntity, TId>
{
    public TId Id { get; private set; } = id;
}