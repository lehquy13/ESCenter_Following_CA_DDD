using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public record RequestTutorCommand(TutorRequestForCreateDto TutorRequestForCreateDto) : ICommandRequest, IAuthorizationRequest;

public class RequestTutorCommandValidator : AbstractValidator<RequestTutorCommand>
{
    public RequestTutorCommandValidator()
    {
        RuleFor(x => x.TutorRequestForCreateDto).NotNull();
        RuleFor(x => x.TutorRequestForCreateDto).SetValidator(new TutorRequestForCreateDtoValidator());
    }
}

public class TutorRequestForCreateDto
{
    public Guid TutorId { get; set; }
    public Guid LearnerId { get; set; }
    public string RequestMessage { get; set; } = null!;
}

public class TutorRequestForCreateDtoValidator : AbstractValidator<TutorRequestForCreateDto>
{
    public TutorRequestForCreateDtoValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
        RuleFor(x => x.LearnerId).NotEmpty();
        RuleFor(x => x.RequestMessage).NotEmpty();
    }
}

