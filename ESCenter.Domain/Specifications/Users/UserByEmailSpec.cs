using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Users;

public class UserByEmailSpec: SpecificationBase<User>
{
    public UserByEmailSpec(string email) : base()
    {
        Criteria = u => u.Email == email;
    }
}