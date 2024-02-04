using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.ChangePassword;

public record ChangePasswordCommand(
    Guid Id,
    string CurrentPassword,
    string NewPassword,
    string ConfirmedPassword
) : ICommandRequest;
