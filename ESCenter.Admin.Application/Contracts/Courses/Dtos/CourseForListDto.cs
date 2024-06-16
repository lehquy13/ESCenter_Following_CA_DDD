using ESCenter.Admin.Application.Contracts.Commons;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public sealed class CourseForListDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = Domain.Shared.Courses.Status.PendingApproval.ToString();
    public string LearningMode { get; set; } = "Offline";

    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}