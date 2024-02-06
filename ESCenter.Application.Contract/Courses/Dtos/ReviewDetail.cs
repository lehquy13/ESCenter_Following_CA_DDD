using ESCenter.Application.Contract.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contract.Courses.Dtos;

public class ReviewDetailDto
{
    public Guid CourseId { get; set; }
    public Guid LearnerId { get; set; }
    public string LearnerName { get; set; } = "";
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}

public class ReviewDto : BasicAuditedEntityDto<int>
{
    public string LearnerName { get; set; } = "";
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}