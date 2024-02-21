using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses;
using Mapster;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public sealed class LearningCourseForListDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}

public class LearningCourseForListDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //Config for Request getting class
        config.NewConfig<Course, LearningCourseForListDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.LearningMode, src => src.LearningMode.ToString())
            .Map(dest => dest, src => src);
    }
}