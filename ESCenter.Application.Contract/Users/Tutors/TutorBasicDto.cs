using ESCenter.Application.Contract.Commons;

namespace ESCenter.Application.Contract.Users.Tutors;
public class TutorBasicDto : BasicAuditedEntityDto<int>
{
    public string Role { get; set; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = null!;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<string> Majors { get; set; } = null!;
    public List<string> TutorVerificationInfoDtos { get; set; } = null!;

}