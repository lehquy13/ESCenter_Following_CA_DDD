using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.Register;

public record RegisterCommand
(
    string Username,
    string Email,
    string PhoneNumber,
    string Password
): ICommandRequest;

