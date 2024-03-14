using ESCenter.Application.Contracts.Profiles;
using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.Paginated;

namespace ESCenter.Client.Models;

public class ProfileViewModel
{
    public UserProfileDto UserDto { get; set; } = new();
    public TutorMinimalBasicDto TutorDto { get; set; } = new();
    public PaginatedList<CourseRequestDto> RequestGettingClassDtos { get; set; } = null!;

    public ChangePasswordRequest ChangePasswordRequest { get; set; } = new();
    public bool IsPartialLoad { get; set; } = false;
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}