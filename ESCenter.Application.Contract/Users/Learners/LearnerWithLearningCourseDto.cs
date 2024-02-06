using ESCenter.Application.Contract.Commons.Primitives.Auditings;
using ESCenter.Application.Contract.Courses.Dtos;

namespace ESCenter.Application.Contract.Users.Learners;

public sealed class LearnerWithLearningCourseDto : BasicAuditedEntityDto<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;

    public string Role { get; set; } = "Learner";
    public List<CourseForListDto> CourseForListDtos = null!;
}