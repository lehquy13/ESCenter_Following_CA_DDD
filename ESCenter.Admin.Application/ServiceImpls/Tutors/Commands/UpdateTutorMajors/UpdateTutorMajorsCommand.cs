using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorMajors;

public record UpdateTutorMajorsCommand(Guid TutorId, List<int> MajorIds) : ICommandRequest;

public class UpdateTutorMajorsCommandValidator : AbstractValidator<UpdateTutorMajorsCommand>
{
    public UpdateTutorMajorsCommandValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
        RuleFor(x => x.MajorIds).NotEmpty();
    }
}