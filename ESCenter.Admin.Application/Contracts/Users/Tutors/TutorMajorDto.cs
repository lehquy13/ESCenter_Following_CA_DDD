namespace ESCenter.Admin.Application.Contracts.Users.Tutors;

public class TutorMajorDto
{
    public Guid TutorId { get; private set; }
    public int SubjectId { get; private set; }
    public string SubjectName { get; private set; } = null!;
}