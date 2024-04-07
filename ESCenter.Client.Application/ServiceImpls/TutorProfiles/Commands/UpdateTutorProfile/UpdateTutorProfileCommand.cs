using ESCenter.Client.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using Mapster;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;

public record UpdateTutorInformationCommand(TutorBasicUpdateForClientDto TutorBasicUpdateDto) : ICommandRequest;

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
        config.NewConfig<TutorBasicUpdateForClientDto, Tutor>()
            .Map(des => des.University, src => src.University)
            .Map(des => des.AcademicLevel, src => src.AcademicLevel);
    }
}