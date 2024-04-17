using ESCenter.Admin.Application.Contracts.Users.Learners;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Commands.CreateStaff;

public record CreateUpdateStaffProfileCommand(
    LearnerForCreateUpdateDto LearnerForCreateUpdateDto
) : ICommandRequest;

public class CreateUpdateStaffProfileCommandValidator : AbstractValidator<CreateUpdateStaffProfileCommand>
{
    public CreateUpdateStaffProfileCommandValidator()
    {
        RuleFor(x => x.LearnerForCreateUpdateDto).NotNull();
        RuleFor(x => x.LearnerForCreateUpdateDto).SetValidator(new LearnerForCreateUpdateDtoValidator());
    }
}
