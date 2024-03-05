using ESCenter.Domain.Aggregates.Subjects;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Mobile.Application.Contracts.Courses.Dtos;

public class SubjectDto : EntityDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class SubjectDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Subject, SubjectDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .IgnoreNonMapped(true);
        config.NewConfig<SubjectDto, Subject>();
    }
}