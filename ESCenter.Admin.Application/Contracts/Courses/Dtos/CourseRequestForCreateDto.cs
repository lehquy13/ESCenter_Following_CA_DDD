namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public class CourseRequestForCreateDto
{
    public Guid CourseId { get; set; }
    public Guid TutorId { get; set; }
}