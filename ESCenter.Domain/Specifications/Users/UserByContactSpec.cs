using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Users;

public class UserByContactSpec: SpecificationBase<User>
{
    public UserByContactSpec(string contact)
    {
        Criteria = u => u.PhoneNumber == contact;
    }
}