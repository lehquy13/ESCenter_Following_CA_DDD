namespace ESCenter.Client.Application.Contracts.Courses.Dtos;

public class CourseRequestForCreateDto
{
    public Guid CourseId { get; init; }
    public Guid TutorId { get; init; }
}