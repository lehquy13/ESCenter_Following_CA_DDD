using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Users.Tutors;

public class TutorProfileCreateDto
{
    public string AcademicLevel { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public List<int> MajorIds { get; set; } = new();
    public bool IsVerified { get; set; } 
}

public class TutorCreateUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TutorProfileCreateDto, Tutor>()
            .Map(dest => dest.AcademicLevel, src => src.AcademicLevel.ToEnum<AcademicLevel>())
            .Map(dest => dest.University, src => src.University);
    }
}