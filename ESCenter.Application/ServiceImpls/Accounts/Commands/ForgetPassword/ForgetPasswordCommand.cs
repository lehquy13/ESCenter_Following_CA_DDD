using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.ForgetPassword;

public record ForgetPasswordCommand
(
    string Email
) : ICommandRequest;