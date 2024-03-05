using ESCenter.Domain.Aggregates.Courses.Entities;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public class CourseRequestCancelDto
{
    public Guid CourseRequestId { get; set; }
    public Guid CourseId { get; set; }
    public string Description { get; set; } = "The course is unavailable now.";
}
public class CourseRequestCancelDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourseRequest, CourseRequestCancelDto>()
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CourseId, src => src.CourseId.Value)
            .Map(dest => dest.CourseRequestId, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}