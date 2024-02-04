using ESCenter.Application.Contracts.Commons.Primitives;
using ESCenter.Application.Contracts.Commons.Primitives.Auditings;
using ESCenter.Application.Contracts.Courses.Dtos;

namespace ESCenter.Application.Contracts.Users.Tutors;
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

public class TutorMinimalBasicDto : BasicAuditedEntityDto<int>
{
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
}