using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Primitives;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.CreateTutor;

public record CreateTutorCommand(TutorCreateDto TutorForCreateDto) : ICommandRequest;

public class CreateTutorCommandValidator : AbstractValidator<CreateTutorCommand>
{
    public CreateTutorCommandValidator()
    {
        RuleFor(x => x.TutorForCreateDto).NotNull();
        RuleFor(x => x.TutorForCreateDto).SetValidator(new TutorCreateDtoValidator());
    }
}


public class TutorCreateDto : EntityDto<Guid>
{
    public LearnerForCreateUpdateDto LearnerForCreateUpdateDto { get; set; } = new();
    public TutorProfileCreateDto TutorProfileCreateDto { get; set; } = new();
}

public class TutorCreateDtoValidator : AbstractValidator<TutorCreateDto>
{
    public TutorCreateDtoValidator()
    {
        RuleFor(x => x.LearnerForCreateUpdateDto).NotNull();
        RuleFor(x => x.LearnerForCreateUpdateDto).SetValidator(new LearnerForCreateUpdateDtoValidator());
        RuleFor(x => x.TutorProfileCreateDto).NotNull();
        RuleFor(x => x.TutorProfileCreateDto).SetValidator(new TutorProfileCreateDtoValidator());
    }
}
