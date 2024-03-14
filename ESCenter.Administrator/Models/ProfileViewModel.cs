using ESCenter.Application.Accounts.Commands.ChangePassword;
using ESCenter.Application.Contracts.Profiles;

namespace ESCenter.Administrator.Models;

public class ProfileViewModel
{
    public UserProfileDto UserProfileDto { get; set; } = new();

    public ChangePasswordCommand ChangePasswordCommand { get; set; } =
        new(Guid.Empty, string.Empty, string.Empty, string.Empty);
}