using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses;
using Mapster;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public sealed class CourseForListDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = "Offline";

    public float Fee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";

    public string LearnerGender { get; set; } = "None";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;

    public float SessionDuration { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    public string Address { get; set; } = string.Empty;

    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}

public class CourseForListDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tuple<Course, string>, CourseForListDto>()
            .Map(dest => dest.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(dest => dest.Status, src => src.Item1.Status.ToString())
            .Map(dest => dest.Fee, src => src.Item1.SectionFee.Amount)
            .Map(dest => dest.ChargeFee, src => src.Item1.ChargeFee.Amount)
            .Map(dest => dest.GenderRequirement, src => src.Item1.GenderRequirement.ToString())
            .Map(dest => dest.AcademicLevelRequirement, src => src.Item1.AcademicLevelRequirement.ToString())
            .Map(dest => dest.SessionDuration, src => src.Item1.SessionDuration.Value)
            .Map(dest => dest.SessionPerWeek, src => src.Item1.SessionPerWeek.Value)
            .Map(dest => dest.SubjectId, src => src.Item1.SubjectId.Value)
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.SubjectName, src => src.Item2)
            .Map(dest => dest.Address, src => src.Item1.Address)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest, src => src);
    }
}