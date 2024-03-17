using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;

public record UpdateTutorInformationCommand(TutorBasicUpdateForClientDto TutorBasicUpdateDto) : ICommandRequest, IAuthorizationRequest;

public class UpdateTutorInformationCommandValidator : AbstractValidator<UpdateTutorInformationCommand>
{
    public UpdateTutorInformationCommandValidator()
    {
        RuleFor(x => x.TutorBasicUpdateDto).NotNull();
        RuleFor(x => x.TutorBasicUpdateDto).SetValidator(new TutorBasicUpdateForClientDtoValidator());
    }
}


public class TutorBasicUpdateForClientDto
{
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.UnderGraduated.ToString();
    public string University { get; set; } = null!;
    public List<string> Majors { get; set; } = new();
}

public class TutorBasicUpdateForClientDtoValidator : AbstractValidator<TutorBasicUpdateForClientDto>
{
    public TutorBasicUpdateForClientDtoValidator()
    {
        RuleFor(dto => dto.AcademicLevel)
            .NotEmpty().WithMessage("Academic level is required.")
            .MaximumLength(100).WithMessage("Academic level must not exceed 100 characters.");

        RuleFor(dto => dto.University)
            .NotEmpty().WithMessage("University is required.");

        RuleForEach(dto => dto.Majors)
            .NotEmpty().WithMessage("Major name must not be empty.")
            .MaximumLength(100).WithMessage("Major name must not exceed 100 characters.");
    }
}