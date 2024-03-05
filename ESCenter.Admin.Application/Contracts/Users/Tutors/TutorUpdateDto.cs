using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Admin.Application.Contracts.Users.Tutors;

public class TutorUpdateDto : LearnerForCreateUpdateDto
{
    public string AcademicLevel { get; init; } = "Student";
    public string University { get; init; } = string.Empty;
    public bool IsVerified { get; init; } = false;
    public short Rate { get; init; } = 5;
    public List<SubjectMajorDto> Majors { get; init; } = new(); 
    public List<VerificationDto> VerificationDtos { get; init; } = new();
    public List<ReviewDetailDto> ReviewDetailDtos { get; init; } = null!; // Currently, it is not used
    public ChangeVerificationRequestDto? ChangeVerificationRequestDto{ get; init; } 
}

public class ChangeVerificationRequestDto 
{
    public Guid Id { get; set; }
    public string RequestStatus { get; set; } = null!;
    public List<string> ChangeVerificationRequestDetails { get; set; } = null!;
}

public class VerificationDto : BasicAuditedEntityDto<Guid>
{
    public string Image { get; init; } = "doc_contract.png";
}

public class SubjectMajorDto : EntityDto<int>
{
    public string Name { get; init; } = string.Empty;
    public bool IsSelected { get; init; }
}

public class TutorForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TutorMajor, SubjectMajorDto>()
            .Map(des => des.Id, src => src.SubjectId.Value)
            .Map(des => des.IsSelected, src => true)
            .Map(des => des.Name, src => src.SubjectName);
        
        config.NewConfig<Subject, SubjectMajorDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des.Name, src => src.Name);

        config.NewConfig<Verification, VerificationDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);
            
        
        config.NewConfig<ChangeVerificationRequest, ChangeVerificationRequestDto>()
            .Map(dest => dest.ChangeVerificationRequestDetails,
                src => src.ChangeVerificationRequestDetails.Select(x => x.ImageUrl).ToList())
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
        
        config.NewConfig<(User, Tutor), TutorUpdateDto>()
            .Map(dest => dest.Id, src => src.Item2.Id.Value)
            .Map(des => des.City, src => src.Item1.Address.City)
            .Map(des => des.Country, src => src.Item1.Address.Country)
            .Map(des => des, src => src.Item1)
            .Map(des => des.Majors, src => src.Item2.TutorMajors)
            .Map(des => des.VerificationDtos, src => src.Item2.Verifications)
            .Map(des => des.ChangeVerificationRequestDto, src => src.Item2.ChangeVerificationRequest)
            .Map(des => des, src => src.Item2);
    }
}