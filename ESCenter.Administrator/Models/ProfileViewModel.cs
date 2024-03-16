using ESCenter.Application.Accounts.Commands.ChangePassword;
using ESCenter.Application.Contracts.Profiles;

namespace ESCenter.Administrator.Models;

public class ProfileViewModel
{
    public UserProfileDto UserProfileDto { get; init; } = new();
    public ChangePasswordCommand ChangePasswordCommand { get; init; } = new(string.Empty, string.Empty, string.Empty);
}