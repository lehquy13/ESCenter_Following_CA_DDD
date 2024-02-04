namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorVerificationInfoForEditDto 
{
    public string TutorId { get; set; }
    public string TutorName { get; set; } = null!;
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = new();
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = new();
}