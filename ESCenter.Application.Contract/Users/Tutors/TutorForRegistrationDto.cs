using ESCenter.Application.Contract.Commons.Primitives;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorForRegistrationDto : EntityDto<Guid>
{
    public string Description { get; set; } = string.Empty;

    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<string> Majors { get; set; } = new();
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = new();

}