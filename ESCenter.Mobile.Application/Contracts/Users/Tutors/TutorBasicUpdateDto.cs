using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Users.Tutors;

public class TutorBasicUpdateDto
{
    public Guid Id { get; set; }
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.UnderGraduated.ToString();
    public string University { get; set; } = null!;
}


public class TutorBasicUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TutorBasicUpdateDto, Tutor>()
            .Map(des => des.University, src => src.University)
            .Map(des => des.AcademicLevel, src => src.AcademicLevel);
        
        config.NewConfig<TutorBasicUpdateForClientDto, Tutor>()
            .Map(des => des.University, src => src.University)
            .Map(des => des.AcademicLevel, src => src.AcademicLevel);
    }
}