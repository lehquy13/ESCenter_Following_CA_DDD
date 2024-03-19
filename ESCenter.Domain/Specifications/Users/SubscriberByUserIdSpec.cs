using ESCenter.Domain.Aggregates.Subscribers;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Users;

public class SubscriberByUserIdSpec: SpecificationBase<Subscriber>
{
    public SubscriberByUserIdSpec(IdentityGuid userId)
    {
        Criteria = u => u.SubscriberId == userId;
    }
}