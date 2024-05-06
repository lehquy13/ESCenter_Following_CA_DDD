using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Courses.Entities;

public class CourseRequest : AuditedEntity<CourseRequestId>
{
    public TutorId TutorId { get; private set; } = null!;
    //public TutorCourseRequest TutorCourseRequest { get; private set; } = null!;
    public CourseId CourseId { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public RequestStatus RequestStatus { get; private set; } = RequestStatus.Pending;

    private CourseRequest()
    {
    }

    public static CourseRequest Create(
        TutorId tutorId,
        CourseId courseId,
        string description)
    {
        return new CourseRequest()
        {
            Id = CourseRequestId.Create(),
            TutorId = tutorId,
            CourseId = courseId,
            Description = description
        };
    }

    public void Cancel(string description = "The course is unavailable now.")
    {
        RequestStatus = RequestStatus.Canceled;
        Description = description;
    }

    public void Approved()
    {
        RequestStatus = RequestStatus.Done;
    }
}