using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Users;

public sealed class UserListQuerySpec : GetPaginatedListSpecificationBase<User>
{
    public UserListQuerySpec(
        int pageIndex,
        int pageSize)
        : base(pageIndex, pageSize)
    {
    }
}