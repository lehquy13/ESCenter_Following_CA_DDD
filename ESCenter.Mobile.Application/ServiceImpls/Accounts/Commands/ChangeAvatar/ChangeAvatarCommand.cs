using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.ChangeAvatar;

public record ChangeAvatarCommand(string Url) : ICommandRequest;

public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
{
    public ChangeAvatarCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            //.Url()
            .WithMessage("Please enter a valid image URL.");
    }
}