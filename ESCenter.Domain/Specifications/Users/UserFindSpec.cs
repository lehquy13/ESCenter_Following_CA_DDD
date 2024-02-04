using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Users;

public class UserFindSpec : FindSpecificationBase<IdentityUser, IdentityGuid>
{
    public UserFindSpec(IdentityGuid id) : base(id)
    {
        IncludeStrings.Add(nameof(IdentityUser.IdentityRole));
        //IncludeStrings.Add(nameof(IdentityUser.User));
    }
}