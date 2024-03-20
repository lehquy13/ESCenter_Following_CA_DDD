using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Customers;

public class CustomerByContactSpec: SpecificationBase<Customer>
{
    public CustomerByContactSpec(string contact)
    {
        Criteria = u => u.PhoneNumber == contact;
    }
}