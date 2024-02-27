using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses.Entities;
using Mapster;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class CourseRequestListForAdminDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Verifying";
}

public class CourseRequestListForAdminDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(string, CourseRequest, string), CourseRequestForListDto>()
            .Map(dest => dest.Id, src => src.Item2.Id.Value)
            .Map(dest => dest.SubjectName, src => src.Item3)
            .Map(dest => dest.Title, src => src.Item1)
            .Map(dest => dest.RequestStatus, src => src.Item2.RequestStatus.ToString())
            .Map(dest => dest.CourseId, src => src.Item2.CourseId.Value);
    }
}