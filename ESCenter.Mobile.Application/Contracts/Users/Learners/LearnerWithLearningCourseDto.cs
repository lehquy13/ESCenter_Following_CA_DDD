using ESCenter.Mobile.Application.Contracts.Commons;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;

namespace ESCenter.Mobile.Application.Contracts.Users.Learners;

public sealed class LearnerWithLearningCourseDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;

    public string Role { get; set; } = "Learner";
    public List<CourseForListDto> CourseForListDtos = null!;
}