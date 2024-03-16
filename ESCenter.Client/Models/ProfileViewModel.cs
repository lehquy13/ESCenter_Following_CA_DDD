using ESCenter.Application.Contracts.Profiles;
using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.Paginated;

namespace ESCenter.Client.Models;

public class ProfileViewModel
{
    public UserProfileDto UserProfileDto { get; init; } = new();
    public TutorMinimalBasicDto TutorMinimalBasicDto { get; init; } = new();
    public IEnumerable<CourseRequestDto>? RequestGettingClassDtos { get; init; } = null!;
    public IEnumerable<LearningCourseForListDto> LearningCourseForListDtos { get; init; } = [];
    public ChangePasswordRequest ChangePasswordRequest { get; init; } = new();
    public bool IsPartialLoad { get; init; } = false;
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
}