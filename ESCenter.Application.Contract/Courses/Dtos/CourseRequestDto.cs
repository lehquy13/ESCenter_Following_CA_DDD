using ESCenter.Application.Contracts.Commons.Primitives.Auditings;
using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class CourseRequestDto : FullAuditedAggregateRootDto<int>
{
    public int TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
}