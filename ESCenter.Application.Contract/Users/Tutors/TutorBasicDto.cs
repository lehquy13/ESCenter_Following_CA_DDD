using ESCenter.Application.Contract.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contract.Users.Tutors;
public class TutorBasicDto : BasicAuditedEntityDto<int>
{
    //is tutor related informtions
    public string Role { get; set; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<string> Majors { get; set; } = new();
    public List<string> TutorVerificationInfoDtos { get; set; } = new();

}