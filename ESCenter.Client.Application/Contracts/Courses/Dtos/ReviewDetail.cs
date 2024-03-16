namespace ESCenter.Client.Application.Contracts.Courses.Dtos;

public class ReviewDetailDto
{
    public Guid CourseId { get; set; }
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}