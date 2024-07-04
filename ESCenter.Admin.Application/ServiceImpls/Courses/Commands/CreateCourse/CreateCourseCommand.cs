using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using FluentValidation;
using Mapster;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CreateCourse;

public record CreateCourseCommand(CourseForCreateDto CourseForCreateDto) : ICommandRequest;

public class CourseForCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = Domain.Shared.Courses.Status.PendingApproval.ToString();
    public string LearningMode { get; set; } = "Offline";
    public decimal Fee { get; set; }
    public decimal ChargeFee { get; set; }
    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";
    public string LearnerGender { get; set; } = "Male";
    public string LearnerName { get; set; } = "";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;
    public int MinutePerSession { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;
    public string Address { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}

public class CourseForCreateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourseForCreateDto, Course>()
            .MapWith(x =>
                Course.Create(
                    x.Title,
                    x.Description,
                    x.LearningMode.ToEnum<LearningMode>(),
                    x.Fee,
                    x.ChargeFee,
                    "Dollar",
                    x.GenderRequirement.ToEnum<Gender>(),
                    x.AcademicLevelRequirement.ToEnum<AcademicLevel>(),
                    x.LearnerGender.ToEnum<Gender>(),
                    x.LearnerName,
                    x.NumberOfLearner,
                    x.ContactNumber,
                    x.MinutePerSession,
                    null,
                    x.SessionPerWeek,
                    x.Address,
                    SubjectId.Create(x.SubjectId),
                    null)
            );
    }
}

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(command => command.CourseForCreateDto)
            .NotNull().WithMessage("CourseForCreateDto is required.");

        RuleFor(command => command.CourseForCreateDto)
            .SetValidator(new CourseForCreateDtoValidator());
    }
}

public class CourseForCreateDtoValidator : AbstractValidator<CourseForCreateDto>
{
    public CourseForCreateDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.Fee)
            .GreaterThan(0).WithMessage("Fee must be greater than 0.");

        RuleFor(dto => dto.ChargeFee)
            .GreaterThan(0).WithMessage("ChargeFee must be greater than 0.");

        RuleFor(dto => dto.ContactNumber)
            .NotEmpty().WithMessage("ContactNumber is required.");

        RuleFor(dto => dto.MinutePerSession)
            .GreaterThan(0).WithMessage("MinutePerSession must be greater than 0.");

        RuleFor(dto => dto.SessionPerWeek)
            .GreaterThan(0).WithMessage("SessionPerWeek must be greater than 0.");

        RuleFor(dto => dto.Address)
            .NotEmpty().WithMessage("Address is required.");

        RuleFor(dto => dto.SubjectId)
            .NotEmpty().WithMessage("SubjectId is required.")
            .GreaterThan(0).WithMessage("SubjectId must be greater than 0.");
    }
}