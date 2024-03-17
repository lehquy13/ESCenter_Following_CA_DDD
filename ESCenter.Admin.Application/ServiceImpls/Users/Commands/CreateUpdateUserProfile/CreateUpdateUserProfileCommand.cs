using ESCenter.Admin.Application.Contracts.Users.Learners;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Users.Commands.CreateUpdateUserProfile;

public record CreateUpdateUserProfileCommand(
    LearnerForCreateUpdateDto LearnerForCreateUpdateDto
) : ICommandRequest;

public class CreateUpdateUserProfileCommandValidator : AbstractValidator<CreateUpdateUserProfileCommand>
{
    public CreateUpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.LearnerForCreateUpdateDto).NotNull();
        RuleFor(x => x.LearnerForCreateUpdateDto).SetValidator(new LearnerForCreateUpdateDtoValidator());
    }
}
