using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.ClearTutorRequests;

public record ClearTutorRequestsCommand(Guid TutorId) : ICommandRequest;

public class ClearTutorRequestsCommandValidator : AbstractValidator<ClearTutorRequestsCommand>
{
    public ClearTutorRequestsCommandValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}