using ESCenter.Domain.Aggregates.CourseRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Courses.CourseRequests;

public class CourseRequest : AuditedEntity<CourseRequestId>
{
    public IdentityGuid TutorId { get; private set; } = null!;
    public CourseId CourseId { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public RequestStatus RequestStatus { get; private set; } = RequestStatus.Pending;

    private CourseRequest()
    {
    }

    public static CourseRequest Create(
        IdentityGuid tutorId,
        CourseId courseId,
        string description)
    {
        return new CourseRequest()
        {
            TutorId = tutorId,
            CourseId = courseId,
            Description = description
        };
    }

    public void Cancel()
    {
        RequestStatus = RequestStatus.Canceled;
    }
}