using ESCenter.Application.Contract.Commons;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorMinimalBasicDto : BasicAuditedEntityDto<Guid>
{
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
}