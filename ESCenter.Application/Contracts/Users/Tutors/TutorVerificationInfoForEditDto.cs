namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorVerificationInfoForEditDto
{
    public Guid TutorId { get; set; }
    public string TutorName { get; set; } = null!;
    public IEnumerable<string> TutorVerificationInfoDtos { get; set; } = null!;
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = null!;
}