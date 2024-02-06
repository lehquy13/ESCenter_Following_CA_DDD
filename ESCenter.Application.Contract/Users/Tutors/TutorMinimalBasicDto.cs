using ESCenter.Application.Contract.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorMinimalBasicDto : BasicAuditedEntityDto<int>
{
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
}