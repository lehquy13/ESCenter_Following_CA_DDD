using Matt.SharedKernel.Domain.Specifications.Interfaces;

namespace Matt.SharedKernel.Domain.Specifications;

public abstract class GetPaginatedListSpecificationBase<TEntity>
    : SpecificationBase<TEntity>, IPaginatedGetListSpecification<TEntity>
{
    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }

    public GetPaginatedListSpecificationBase(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    }
}