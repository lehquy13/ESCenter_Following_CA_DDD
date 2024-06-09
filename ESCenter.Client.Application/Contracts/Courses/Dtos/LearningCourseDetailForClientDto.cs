using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Client.Application.Contracts.Courses.Dtos;

public class LearningCourseDetailForClientDto : BasicAuditedEntityDto<Guid>
{
    //Basic Information
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = Domain.Shared.Courses.Status.PendingApproval.ToString();
    public string LearningMode { get; set; } = Domain.Shared.Courses.LearningMode.Online.ToString();
    public decimal SectionFee { get; set; } = 0;
    public decimal ChargeFee { get; set; } = 0;
    public int SessionDuration { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;
    public string Address { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    
    public string Detail { get; set; } = string.Empty;
    public short Rate { get; set; }
    
    public Guid TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorContact { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
}

public class LearningCourseForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Course, Subject, Guid, Customer), LearningCourseDetailForClientDto>()
            .Map(des => des.Id, src => src.Item1.Id.Value)
            .Map(des => des.TutorId, src => src.Item3)
            .Map(des => des.TutorName, src => src.Item4.GetFullName())
            .Map(des => des.TutorContact, src => src.Item4.Description)
            .Map(des => des.TutorEmail, src => src.Item4.Email)
            .Map(des => des.Title, src => src.Item1.Title)
            .Map(des => des.Status, src => src.Item1.Status.ToString())
            .Map(des => des.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(des => des.ChargeFee, src => src.Item1.ChargeFee.Amount)
            .Map(des => des.SectionFee, src => src.Item1.SectionFee.Amount)
            .Map(des => des.SessionDuration, src => src.Item1.SessionDuration.Value)
            .Map(des => des.SessionPerWeek, src => src.Item1.SessionPerWeek.Value)
            .Map(des => des.SubjectName, src => src.Item2.Name)
            .Map(des => des.Address, src => src.Item1.Address)
            .Map(des => des.Description, src => src.Item1.Description)
            .Map(des => des.Detail, src => src.Item1.Review == null ? "" : src.Item1.Review.Detail)
            .Map(des => des.Rate, src => src.Item1.Review == null ? (short)5 : src.Item1.Review.Rate)
            .Map(des => des, src => src);
    }
}