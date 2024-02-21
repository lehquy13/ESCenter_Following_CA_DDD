namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorBasicForUpdateDto
{
    public Guid Id { get; set; }
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.Student.ToString();
    public string University { get; set; } = null!;
    public List<string> Majors { get; set; } = null!;
}