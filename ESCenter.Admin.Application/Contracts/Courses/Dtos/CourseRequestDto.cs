using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Shared.Courses;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public class CourseRequestDto : BasicAuditedEntityDto<Guid>
{
    public Guid TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
}

public class CourseRequestDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourseRequest, CourseRequestDto>()
            .Map(dest => dest.TutorId, src => src.TutorId.Value)
            .Map(dest => dest.CourseId, src => src.CourseId.Value)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}