using ESCenter.Domain.Aggregates.Subscriptions.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Subscriptions;

public class Subscription : AuditedAggregateRoot<SubscriptionId>
{
    private Subscription()
    {
    }

    public static Subscription Create(CustomerId customerId)
    {
        var tutorRequest = new Subscription()
        {
            Id = SubscriptionId.Create(),
            CustomerId = customerId
        };

        tutorRequest.DomainEvents.Add(new SubscriptionCreatedDomainEvent(tutorRequest));

        return tutorRequest;
    }

    public CustomerId CustomerId { get; private init; } = null!;
    public SubscriptionType SubscriptionType { get; private set; } = SubscriptionType.Basic;
}

public enum SubscriptionType
{
    Basic, // 2 courses each month
    Plus, // 4 courses each month - Machine Learning Searching - Mailing about new courses
          // - Discount on 10% 1 first courses 
    Premium // Unlimited courses each month - Machine Learning Searching - Mailing about new courses
            // - Higher priority when requesting courses - Discount on 15% 2 first courses
}

public record SubscriptionCreatedDomainEvent(Subscription TutorRequest) : IDomainEvent;