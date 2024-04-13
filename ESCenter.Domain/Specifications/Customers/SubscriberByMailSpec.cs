using ESCenter.Domain.Aggregates.Subscribers;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Customers;

public class SubscriberByMailSpec: SpecificationBase<Subscriber>
{
    public SubscriberByMailSpec(string email)
    {
        Criteria = u => u.Email == email;
    }
}