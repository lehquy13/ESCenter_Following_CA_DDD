using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using FluentValidation;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Courses.Dtos;

public class CourseForLearnerCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LearningMode { get; set; } = "Offline";
    public float Fee { get; set; }
    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";
    public string LearnerGender { get; set; } = "Male";
    public string LearnerName { get; set; } = "Male";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;
    public Guid LearnerId { get; set; }
    public int MinutePerSession { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;
    public string Address { get; set; } = string.Empty;
    public int SubjectId { get; set; } = 0;
    public string SubjectName { get; set; } = string.Empty;
}

public class CourseForLearnerCreateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourseForLearnerCreateDto, Course>()
            .ConstructUsing(x =>
                Course.Create(
                    x.Title,
                    x.Description,
                    x.LearningMode.ToEnum<LearningMode>(),
                    x.Fee,
                    x.Fee,
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
                    CustomerId.Create(x.LearnerId))
            );
    }
}

public class CourseForLearnerCreateDtoValidator : AbstractValidator<CourseForLearnerCreateDto>
{
    public CourseForLearnerCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must be less than 100 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must be less than 500 characters long.");

        RuleFor(x => x.LearningMode)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Learning mode must be a valid option.");

        RuleFor(x => x.Fee)
            .InclusiveBetween(0, float.MaxValue)
            .WithMessage("Fee must be a non-negative number.");

        RuleFor(x => x.GenderRequirement)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Gender requirement must be a valid option.");

        RuleFor(x => x.AcademicLevelRequirement)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Academic level requirement must be a valid option.");

        RuleFor(x => x.LearnerGender)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Learner gender must be a valid option.");

        RuleFor(x => x.LearnerName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Learner name must be less than 100 characters long.");

        RuleFor(x => x.NumberOfLearner)
            .InclusiveBetween(1, 100)
            .WithMessage("Number of learners must be between 1 and 100.");

        RuleFor(x => x.ContactNumber)
            .Matches(@"^\d{10}$") // Assuming 10-digit phone number
            .WithMessage("Contact number must be 10 digits long.");

        // RuleFor(x => x.LearnerId)
        //     .NotEmpty()
        //     .WithMessage("Learner ID is required.");

        RuleFor(x => x.MinutePerSession)
            .InclusiveBetween(1, 180)
            .WithMessage("Minutes per session must be between 1 and 180.");

        RuleFor(x => x.SessionPerWeek)
            .InclusiveBetween(1, 7)
            .WithMessage("Sessions per week must be between 1 and 7.");

        RuleFor(x => x.Address)
            .MaximumLength(255)
            .WithMessage("Address must be less than 255 characters long.");

        RuleFor(x => x.SubjectId)
            .GreaterThan(0)
            .WithMessage("Subject ID must be a positive number.");

        // RuleFor(x => x.SubjectName)
        //     .NotEmpty()
        //     .MaximumLength(100)
        //     .WithMessage("Subject name must be less than 100 characters long.");
    }
}