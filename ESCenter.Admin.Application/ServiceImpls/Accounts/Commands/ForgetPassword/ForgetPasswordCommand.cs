using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Commands.ForgetPassword;

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
            //.Url()
            .WithMessage("Please enter a valid email.");
    }
}