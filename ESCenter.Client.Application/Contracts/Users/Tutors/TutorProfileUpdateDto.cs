using ESCenter.Domain.Aggregates.Tutors;
using Mapster;

namespace ESCenter.Client.Application.Contracts.Users.Tutors;

public class TutorProfileUpdateDto
{
    public Guid Id { get; set; }
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.UnderGraduated.ToString();
    public string University { get; set; } = null!;
}

public class TutorBasicUpdateForClientDto
{
    public Guid Id { get; set; }
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.UnderGraduated.ToString();
    public string University { get; set; } = null!;
    public List<string> Majors { get; set; } = new();
}

public class TutorBasicUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TutorProfileUpdateDto, Tutor>()
            .Map(des => des.University, src => src.University)
            .Map(des => des.AcademicLevel, src => src.AcademicLevel);
        
        config.NewConfig<TutorBasicUpdateForClientDto, Tutor>()
            .Map(des => des.University, src => src.University)
            .Map(des => des.AcademicLevel, src => src.AcademicLevel);
    }
}