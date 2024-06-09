using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using FluentValidation;
using Mapster;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand(CourseUpdateDto CourseUpdateDto) : ICommandRequest;

public class CourseUpdateDto
{
    //Basic Information
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = Domain.Shared.Courses.Status.PendingApproval.ToString();
    public string LearningMode { get; set; } = Domain.Shared.Courses.LearningMode.Offline.ToString();

    public decimal SectionFee { get; set; } = 0;
    public decimal ChargeFee { get; set; } = 0;

    //Tutor related information
    public string GenderRequirement { get; set; } = Gender.None.ToString();
    public string AcademicLevelRequirement { get; set; } = AcademicLevel.Optional.ToString();

    //Student related information
    public string LearnerName { get; set; } = "";
    public string LearnerGender { get; set; } = Gender.Male.ToString();
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;

    // Time related information
    public int SessionDuration { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    // Address related information
    public string Address { get; set; } = string.Empty;

    //Subject related information
    public int SubjectId { get; set; }
}

public class CourseUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourseUpdateDto, Course>()
            .Map(dest => dest.Id,
                src => src.Id == Guid.Empty
                    ? CourseId.Create(Guid.Empty)
                    : CourseId.Create(src.Id))
            .Map(dest => dest.LearningMode, src => src.LearningMode.ToEnum<LearningMode>())
            .Map(dest => dest.GenderRequirement, src => src.GenderRequirement.ToEnum<Gender>())
            .Map(dest => dest.AcademicLevelRequirement, src => src.AcademicLevelRequirement.ToEnum<AcademicLevel>())
            .Map(dest => dest.SectionFee, src => Fee.Create(src.SectionFee, Currency.USD))
            .Map(dest => dest.ChargeFee, src => Fee.Create(src.ChargeFee, Currency.USD))
            .Map(dest => dest.SessionDuration, src => SessionDuration.Create(src.SessionDuration, null))
            .Map(dest => dest.SessionPerWeek, src => SessionPerWeek.Create(src.SessionPerWeek))
            .Map(dest => dest.NumberOfLearner, src => src.NumberOfLearner)
            .Map(dest => dest.Status, src => src.Status.ToEnum<Status>())
            .Map(dest => dest.SubjectId, src => SubjectId.Create(src.SubjectId))
            // .Map(dest => dest.TutorId, src => src.TutorId != Guid.Empty ? CustomerId.Create(src.TutorId) : null)
            // .Map(dest => dest.Status, src => src.TutorId == Guid.Empty ? src.Status.ToEnum<Status>() : Status.Confirmed)
            .Map(dest => dest, src => src);
    }
}

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(command => command.CourseUpdateDto)
            .NotNull().WithMessage("CourseUpdateDto is required.");

        RuleFor(command => command.CourseUpdateDto)
            .SetValidator(new CourseUpdateDtoValidator());
    }
}

public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
{
    public CourseUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(dto => dto.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(dto => dto.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(dto => dto.Status).NotEmpty().WithMessage("Status is required.");
        RuleFor(dto => dto.LearningMode).NotEmpty().WithMessage("LearningMode is required.");
        RuleFor(dto => dto.SectionFee).GreaterThanOrEqualTo(0)
            .WithMessage("SectionFee must be greater than or equal to 0.");
        RuleFor(dto => dto.ChargeFee).GreaterThanOrEqualTo(0)
            .WithMessage("ChargeFee must be greater than or equal to 0.");
        RuleFor(dto => dto.GenderRequirement).NotEmpty().WithMessage("GenderRequirement is required.");
        RuleFor(dto => dto.AcademicLevelRequirement).NotEmpty().WithMessage("AcademicLevelRequirement is required.");
        RuleFor(dto => dto.LearnerName).NotEmpty().WithMessage("LearnerName is required.");
        RuleFor(dto => dto.LearnerGender).NotEmpty().WithMessage("LearnerGender is required.");
        RuleFor(dto => dto.NumberOfLearner).GreaterThan(0).WithMessage("NumberOfLearner must be greater than 0.");
        RuleFor(dto => dto.ContactNumber).NotEmpty().WithMessage("ContactNumber is required.");
        RuleFor(dto => dto.SessionDuration).GreaterThan(0).WithMessage("SessionDuration must be greater than 0.");
        RuleFor(dto => dto.SessionPerWeek).GreaterThan(0).WithMessage("SessionPerWeek must be greater than 0.");
        RuleFor(dto => dto.Address).NotEmpty().WithMessage("Address is required.");
        RuleFor(dto => dto.SubjectId).NotEmpty().WithMessage("SubjectId is required.");
    }
}