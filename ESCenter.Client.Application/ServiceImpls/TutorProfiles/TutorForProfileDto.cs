using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using Mapster;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles;

public class TutorForProfileDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Role { get; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public float Rate { get; set; } 
    public List<TutorMajorDto> Majors { get; set; } = new(); 
    public List<VerificationDto> VerificationDtos { get; set; } = new();
    public List<ChangeVerificationRequestDto> ChangeVerificationRequestDtos { get; set; } = new();
    public List<BasicCourseRequestDto> BasicCourseRequests { get; set; } = new();
}

public class VerificationDto : BasicAuditedEntityDto<Guid>
{
    public string Image { get; init; } = "doc_contract.png";
}

public class BasicCourseRequestDto : BasicAuditedEntityDto<Guid>
{
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Pending";
}

public class ChangeVerificationRequestDto 
{
    public Guid Id { get; set; }
    public string RequestStatus { get; set; } = null!;
    public List<string> ChangeVerificationRequestDetails { get; set; } = null!;
}

public class TutorMajorDto
{
    public bool IsMajored { get;  set; }
    public int SubjectId { get;  set; }
    public string SubjectName { get;  set; } = null!;
}

public class TutorForProfileDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ChangeVerificationRequest, ChangeVerificationRequestDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ChangeVerificationRequestDetails, src => src.ChangeVerificationRequestDetails.Select(x => x.ImageUrl).ToList())
            .Map(dest => dest.RequestStatus, src => src.RequestStatus.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<Tutor, TutorForProfileDto>()
            .Map(dest => dest.AcademicLevel, src => src.AcademicLevel.ToString())
            .Map(dest => dest.ChangeVerificationRequestDtos, src => src.ChangeVerificationRequest)
            .Map(dest => dest.VerificationDtos, src => src.Verifications)
            .Map(dest => dest.Majors, src => src.TutorMajors)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}