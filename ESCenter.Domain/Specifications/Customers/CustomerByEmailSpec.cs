using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Customers;

public class CustomerByEmailSpec: SpecificationBase<Customer>
{
    public CustomerByEmailSpec(string email)
    {
        Criteria = u => u.Email == email;
    }
}