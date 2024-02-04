using ESCenter.Application.Contracts.Commons.Primitives.Auditings;
using ESCenter.Application.Contracts.Courses.Dtos;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorForProfileDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string Role { get; set; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 0;
    public List<SubjectDto> Majors { get; set; } = new();
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = new();
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = new();
    public List<CourseRequestDto> CourseRequestDtos { get; set; } = new();
}