using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Users.Tutors;

public class TutorMinimalBasicDto : BasicAuditedEntityDto<Guid>
{
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<TutorMajorDto> Majors { get; set; } = new();
    public List<VerificationDto> VerificationDtos { get; set; } = new();
}

public class VerificationDto : BasicAuditedEntityDto<Guid>
{
    public string Image { get; init; } = "doc_contract.png";
}

public class TutorMajorDto
{
    public int SubjectId { get; private set; }
    public string SubjectName { get; private set; } = null!;
}

public class TutorMinimalBasicDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tutor, TutorMinimalBasicDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Majors, src => src.TutorMajors)
            .Map(dest => dest.AcademicLevel, src => src.AcademicLevel.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<TutorMajor, TutorMajorDto>()
            .Map(dest => dest.SubjectId, src => src.SubjectId.Value)
            .Map(dest => dest.SubjectName, src => src.SubjectName)
            .Map(dest => dest, src => src);
    }
}