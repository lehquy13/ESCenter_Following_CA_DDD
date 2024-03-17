using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateChangeVerificationRequestCommand;

public record UpdateChangeVerificationCommand(Guid TutorId, bool IsApproved) : ICommandRequest;

public class UpdateChangeVerificationCommandValidator : AbstractValidator<UpdateChangeVerificationCommand>
{
    public UpdateChangeVerificationCommandValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}
