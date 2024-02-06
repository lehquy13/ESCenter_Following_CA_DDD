using ESCenter.Application.Contract.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contract.Courses.Dtos;

public sealed class LearningCourseForListDto : BasicAuditedEntityDto<int>
{
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}