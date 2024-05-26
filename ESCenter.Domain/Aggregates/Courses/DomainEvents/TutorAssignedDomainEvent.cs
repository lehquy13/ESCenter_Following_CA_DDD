using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.Aggregates.Courses.DomainEvents;

public record TutorAssignedDomainEvent(Course Course, TutorId TutorId) : IDomainEvent;