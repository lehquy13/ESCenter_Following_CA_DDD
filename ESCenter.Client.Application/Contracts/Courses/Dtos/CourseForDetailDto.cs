using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Client.Application.Contracts.Courses.Dtos;

public class CourseForDetailDto : BasicAuditedEntityDto<Guid>
{
    //Basic Information
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

    // Time related information
    public int SessionDuration { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    // Address related information
    public string Address { get; set; } = string.Empty;

    //Subject related information
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}


public class CourseForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tuple<Course, Subject>, CourseForDetailDto>()
            .Map(des => des.Id, src => src.Item1.Id.Value)
            .Map(des => des.LearnerName, src => src.Item1.LearnerName)
            .Map(des => des.Title, src => src.Item1.Title)
            .Map(des => des.Status, src => src.Item1.Status.ToString())
            .Map(des => des.GenderRequirement, src => src.Item1.GenderRequirement.ToString())
            .Map(des => des.LearnerGender, src => src.Item1.LearnerGender.ToString())
            .Map(des => des.AcademicLevelRequirement, src => src.Item1.AcademicLevelRequirement.ToString())
            .Map(des => des.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(des => des.ChargeFee, src => src.Item1.ChargeFee.Amount)
            .Map(des => des.SectionFee, src => src.Item1.SectionFee.Amount)
            .Map(des => des.SessionDuration, src => src.Item1.SessionDuration.Value)
            .Map(des => des.SessionPerWeek, src => src.Item1.SessionPerWeek.Value)
            .Map(des => des.SubjectName, src => src.Item2.Name)
            .Map(des => des.SubjectId, src => src.Item2.Id.Value)
            .Map(des => des.Address, src => src.Item1.Address)
            .Map(des => des.Description, src => src.Item1.Description)
            .Map(des => des.NumberOfLearner, src => src.Item1.NumberOfLearner)
            .Map(des => des, src => src);
    }
}