using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.Aggregates.Courses.DomainEvents;

public record CourseReviewedDomainEvent(Course Course) : IDomainEvent;