using ESCenter.Domain.Aggregates.Subscribers;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Customers;

public class SubscriberByUserIdSpec: SpecificationBase<Subscriber>
{
    public SubscriberByUserIdSpec(CustomerId userId)
    {
        Criteria = u => u.SubscriberId == userId;
    }
}