using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.ResetPassword;

public record ResetPasswordCommand
(
    string Email,
    string Otp,
    string NewPassword
) : ICommandRequest;