namespace ESCenter.Application.Contracts.Users.Tutors;

public class ChangeVerificationRequestDto 
{
    public int Id { get; set; }
    
    public int TutorId { get; set; }
    
    public string RequestStatus { get; set; } = null!;

    public List<string> ChangeVerificationRequestDetails { get; set; } = new();
}