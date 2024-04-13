using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.Aggregates.TutorRequests.DomainEvents;

public record TutorRequestCreatedDomainEvent(TutorRequest TutorRequest) : IDomainEvent;