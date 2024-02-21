using ESCenter.Application.Contracts.Commons;
using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorMajors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using Mapster;

namespace ESCenter.Application.ServiceImpls.Clients.Tutors.Queries;

public class TutorForProfileDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string Role { get; set; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 0;
    public List<TutorMajorDto> Majors { get; set; } = new(); 
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = new();
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = new();
    public List<CourseRequestDto> CourseRequestDtos { get; set; } = new(); // TODO: Add CourseRequestDto
}

public class TutorForProfileDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TutorMajor, TutorMajorDto>()
            .Map(dest => dest.TutorId, src => src.TutorId.Value)
            .Map(dest => dest.SubjectId, src => src.SubjectId.Value)
            .Map(dest => dest, src => src);

        
        config.NewConfig<Tutor, TutorForProfileDto>()
            .Map(dest => dest.ChangeVerificationRequestDtos, src => src.ChangeVerificationRequests)
            .Map(dest => dest.TutorVerificationInfoDtos, src => src.TutorVerificationInfos)
            .Map(dest => dest.Majors, src => src.TutorMajors)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}