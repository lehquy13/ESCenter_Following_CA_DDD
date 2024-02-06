namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorVerificationInfoForEditDto
{
    public string TutorId { get; set; } = null!;
    public string TutorName { get; set; } = null!;
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = null!;
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = null!;
}