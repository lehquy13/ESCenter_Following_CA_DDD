using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Courses.Dtos;

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
        config.NewConfig<(Course, Subject), LearningCourseForListDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(dest => dest.LearningMode, src => src.Item2.Id.Value)
            .Map(dest => dest.Title, src => src.Item1.Title.ToString())
            .Map(dest => dest.Status, src => src.Item1.Status.ToString())
            .Map(dest => dest.SubjectName, src => src.Item2.Name);
    }
}