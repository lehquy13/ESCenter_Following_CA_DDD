namespace ESCenter.Admin.Application.Contracts.Users.Tutors;

public class VerificationEditDto
{
    public Guid TutorId { get; set; }
    public string TutorName { get; set; } = null!;
    public IEnumerable<string> VerificationDtos { get; set; } = null!;
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = null!;
}
