using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Tutors;
using Mapster;

namespace ESCenter.Client.Application.Contracts.Users.Tutors;

public class TutorMinimalBasicDto : BasicAuditedEntityDto<Guid>
{
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
}

public class TutorMinimalBasicDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tutor, TutorMinimalBasicDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.AcademicLevel, src => src.AcademicLevel.ToString())
            .Map(dest => dest, src => src);
    }
}