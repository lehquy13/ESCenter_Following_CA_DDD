using Matt.SharedKernel.Domain.Specifications.Interfaces;

namespace Matt.SharedKernel.Domain.Specifications;

public abstract class GetListSpecificationBase<TEntity>
    : SpecificationBase<TEntity>, IGetListSpecification<TEntity>
{
    public GetListSpecificationBase()
    {
    }
}