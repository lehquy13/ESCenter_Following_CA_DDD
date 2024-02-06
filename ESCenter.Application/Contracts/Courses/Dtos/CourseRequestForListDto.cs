using ESCenter.Application.Contracts.Commons;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class CourseRequestForListDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Verifying";
}