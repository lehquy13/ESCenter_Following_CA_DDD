using ESCenter.Application.Contracts.Commons;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class ReviewDto : BasicAuditedEntityDto<int>
{
    public string LearnerName { get; set; } = "";
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}