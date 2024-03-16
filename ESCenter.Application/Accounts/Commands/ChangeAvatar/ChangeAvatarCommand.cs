using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.Accounts.Commands.ChangeAvatar;

public record ChangeAvatarCommand(string Url) : ICommandRequest, IAuthorizationRequest;

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