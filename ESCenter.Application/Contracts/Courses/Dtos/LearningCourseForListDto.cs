using ESCenter.Application.Contracts.Commons;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public sealed class LearningCourseForListDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}