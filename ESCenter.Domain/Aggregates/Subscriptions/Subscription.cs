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
            LearnerId = customerId
        };

        tutorRequest.DomainEvents.Add(new SubscriptionCreatedDomainEvent(tutorRequest));

        return tutorRequest;
    }

    public CustomerId LearnerId { get; private init; } = null!;
}

public record SubscriptionCreatedDomainEvent(Subscription TutorRequest) : IDomainEvent;