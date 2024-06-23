using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorInformation;

public record UpdateTutorInformationCommand(TutorBasicUpdateDto TutorBasicUpdateDto) : ICommandRequest;

public class UpdateTutorInformationCommandValidator : AbstractValidator<UpdateTutorInformationCommand>
{
    public UpdateTutorInformationCommandValidator()
    {
        RuleFor(x => x.TutorBasicUpdateDto).NotNull();
        RuleFor(x => x.TutorBasicUpdateDto).SetValidator(new TutorBasicUpdateDtoValidator());
    }
}

public class TutorBasicUpdateDto
{
    public Guid Id { get; set; }
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.UnderGraduated.ToString();
    public string University { get; set; } = null!;
    public bool IsVerified { get; set; } = false;
}

public class TutorBasicUpdateDtoValidator : AbstractValidator<TutorBasicUpdateDto>
{
    public TutorBasicUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id)
            .NotEmpty().WithMessage("Tutor ID is required.");

        RuleFor(dto => dto.AcademicLevel)
            .NotEmpty().WithMessage("Academic level is required.")
            .MaximumLength(100).WithMessage("Academic level must not exceed 100 characters.");

        RuleFor(dto => dto.University)
            .NotEmpty().WithMessage("University is required.");
    }
}