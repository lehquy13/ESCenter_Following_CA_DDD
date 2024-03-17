using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.Accounts.Commands.ForgetPassword;

public record ForgetPasswordCommand
(
    string Email
) : ICommandRequest;

public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Please enter a valid email.");
    }
}