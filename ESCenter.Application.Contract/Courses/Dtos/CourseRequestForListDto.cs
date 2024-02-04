using ESCenter.Application.Contracts.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class CourseRequestForListDto : BasicAuditedEntityDto<int>
{
    public string Title { get; set; } = string.Empty;

    public int CourseId { get; set; }

    //public CourseDto CourseDto { get; set; } = null!;
    public string SubjectName { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Verifying";
}