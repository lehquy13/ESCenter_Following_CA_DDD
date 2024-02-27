using ESCenter.Application.Contracts.Profiles;
using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.ServiceImpls.Accounts.Commands.ChangePassword;

namespace ESCenter.Administrator.Models;

public class ProfileViewModel
{
    public UserProfileDto UserProfileDto { get; set; } = new();

    public ChangePasswordCommand ChangePasswordCommand { get; set; } =
        new(Guid.Empty, string.Empty, string.Empty, string.Empty);
}