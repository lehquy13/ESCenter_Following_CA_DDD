using ESCenter.Domain.Aggregates.Users.Identities;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Identities;

public class IdentityGetByEmailSpec : SpecificationBase<IdentityUser>
{
    public IdentityGetByEmailSpec(string email)
    {
        Criteria = user => user.Email == email;
    }
}