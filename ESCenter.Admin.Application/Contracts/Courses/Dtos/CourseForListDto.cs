using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

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
        config.NewConfig<(Course, string), CourseForListDto>()
            .Map(des => des.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(des => des.Status, src => src.Item1.Status.ToString())
            .Map(des => des.Fee, src => src.Item1.SectionFee.Amount)
            .Map(des => des.ChargeFee, src => src.Item1.ChargeFee.Amount)
            .Map(des => des.GenderRequirement, src => src.Item1.GenderRequirement)
            .Map(des => des.AcademicLevelRequirement, src => src.Item1.AcademicLevelRequirement.ToString())
            .Map(des => des.SessionDuration, src => src.Item1.SessionDuration.Value)
            .Map(des => des.SessionPerWeek, src => src.Item1.SessionPerWeek.Value)
            .Map(des => des.SubjectId, src => src.Item1.SubjectId.Value)
            .Map(des => des.Id, src => src.Item1.Id.Value)
            .Map(des => des.Address, src => src.Item1.Address)
            .Map(des => des.Title, src => src.Item1.Title)
            .Map(des => des.SubjectName, src => src.Item2)
            .Map(des => des, src => src);
    }
}