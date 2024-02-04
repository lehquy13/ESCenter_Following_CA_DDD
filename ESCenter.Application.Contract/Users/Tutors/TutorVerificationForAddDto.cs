namespace ESCenter.Application.Contracts.Users.Tutors;

public class StreamWithFileName
{
    public Stream Stream { get; set; } = null!;
    public string FileName { get; set; } = null!;
}

public class TutorVerificationForAddDto 
{
    public Guid TutorId { get; set; }
    public List<string> Images { get; set; } = null!;
}