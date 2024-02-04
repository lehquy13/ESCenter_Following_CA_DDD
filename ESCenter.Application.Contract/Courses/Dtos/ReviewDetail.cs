
namespace ESCenter.Application.Contracts.Courses.Dtos;

public class ReviewDetailDto 
{
    public Guid CourseId { get; set; }
    public Guid LearnerId { get; set; }
    public string LearnerName { get; set; } = "";
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}
