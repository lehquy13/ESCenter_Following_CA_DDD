using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public class CourseUpdateDto
{
    //Basic Information
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = "Offline";

    public float SectionFee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    //Tutor related information
    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";

    //Student related information
    public string LearnerName { get; set; } = "";
    public string LearnerGender { get; set; } = "None";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;

    // Time related information
    public int SessionDuration { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    // Address related information
    public string Address { get; set; } = string.Empty;

    //Subject related information
    public int SubjectId { get; set; }

    //Confirmed data related
    public Guid TutorId { get; set; }
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
            .Map(dest => dest.SubjectId, src => SubjectId.Create(src.SubjectId))
            .Map(dest => dest.TutorId, src => src.TutorId != Guid.Empty ? IdentityGuid.Create(src.TutorId) : null)
            .Map(dest => dest.Status, src => src.TutorId == Guid.Empty ? src.Status.ToEnum<Status>() : Status.Confirmed)
            .Map(dest => dest, src => src);
    }
}