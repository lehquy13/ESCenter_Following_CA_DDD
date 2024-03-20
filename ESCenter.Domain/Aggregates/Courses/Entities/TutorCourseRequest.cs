using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.Entities;

public class TutorCourseRequest : Entity<TutorId>
{
    public CourseRequestId CourseRequestId { get; private set; } = null!;
    public AcademicLevel AcademicLevel { get; private set; }
    public string University { get; private set; } = null!;
    public TutorUserInfo TutorUserInfo { get; private set; } = null!;
}

public class TutorUserInfo : Entity<CustomerId>
{
    public TutorId TutorId { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
}