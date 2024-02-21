using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;

namespace ESCenter.Application.Contracts.Courses.Dtos;

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
                    x.LearnerGender,
                    x.LearnerName,
                    x.NumberOfLearner,
                    x.ContactNumber,
                    x.MinutePerSession,
                    null,
                    x.SessionPerWeek,
                    x.Address,
                    SubjectId.Create(x.SubjectId),
                    IdentityGuid.Create(x.LearnerId))
            );
    }
}