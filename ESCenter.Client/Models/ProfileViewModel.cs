using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.ServiceImpls.Notifications;
using ESCenter.Client.Application.ServiceImpls.Payments.Queries.Gets;
using ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetTutoringRequests;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles;

namespace ESCenter.Client.Models;

public class ProfileViewModel
{
    public UserProfileDto UserProfileDto { get; init; } = new();
    public IEnumerable<LearningCourseForListDto> LearningCourseForListDtos { get; init; } = [];
    public IEnumerable<TutorRequestForListDto> TutoringRequestForListDtos { get; init; } = [];
    public List<NotificationDto> NotificationDtos { get; init; } = [];
    public ChangePasswordRequest ChangePasswordRequest { get; init; } = new();
    public bool IsPartialLoad { get; init; } = false;
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmedPassword { get; set; } = string.Empty;
}

public class TutorProfileViewModel
{
    public UserProfileDto UserProfileDto { get; init; } = new();

    public TutorForProfileDto TutorForProfileDto { get; init; } = new();
    public IEnumerable<PaymentDto> PaymentDtos { get; set; } = [];
}