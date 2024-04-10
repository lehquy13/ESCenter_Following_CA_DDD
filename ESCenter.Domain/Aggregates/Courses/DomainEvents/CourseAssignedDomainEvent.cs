using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.Aggregates.Courses.DomainEvents;

public record CourseAssignedDomainEvent(Course Course, TutorId TutorId) : IDomainEvent;