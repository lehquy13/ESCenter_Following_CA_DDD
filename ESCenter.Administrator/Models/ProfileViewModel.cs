using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.ServiceImpls.Accounts.Commands.ChangePassword;

namespace ESCenter.Administrator.Models;

public class ProfileViewModel
{
    public LearnerForProfileDto LearnerForProfileDto { get; set; } = new();

    public ChangePasswordCommand ChangePasswordRequest { get; set; } =
        new(Guid.Empty, string.Empty, string.Empty, string.Empty);
}