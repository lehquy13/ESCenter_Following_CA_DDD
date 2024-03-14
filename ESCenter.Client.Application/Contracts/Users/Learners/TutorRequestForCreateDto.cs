namespace ESCenter.Client.Application.Contracts.Users.Learners;

public class TutorRequestForCreateDto
{
    public Guid TutorId { get; set; }
    public Guid LearnerId { get; set; }
    public string RequestMessage { get; set; } = null!;
}