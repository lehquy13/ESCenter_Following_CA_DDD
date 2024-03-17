using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.Accounts.Commands.ChangePassword;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmedPassword
) : ICommandRequest, IAuthorizationRequest;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Current password must be at least 8 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage(
                "New password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.");

        RuleFor(x => x.ConfirmedPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword)
            .WithMessage("Confirmed password must match the new password.");
    }
}