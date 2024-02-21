using ESCenter.Application.Contracts.Commons;
using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorUpdateDto : LearnerForCreateUpdateDto
{
    //Tutor's related informations
    public string Role { get; set; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<SubjectMajorDto> Majors { get; set; } = new(); // Currently, it is not used
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = new(); // Currently, it is not used
    public List<ReviewDetailDto> ReviewDetailDtos { get; set; } = null!; // Currently, it is not used
}

public class TutorVerificationInfoDto : BasicAuditedEntityDto<Guid>
{
    public Guid TutorId { get; set; }
    public string Image { get; set; } = "doc_contract.png";
}

public class SubjectMajorDto : EntityDto<int>
{
    public string Name { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
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
        
        config.NewConfig<(User, Tutor), TutorUpdateDto>()
            .Map(dest => dest.Id, src => src.Item2.Id.Value)
            .Map(des => des.City, src => src.Item1.Address.City)
            .Map(des => des.Country, src => src.Item1.Address.Country)
            .Map(des => des, src => src.Item1)
            .Map(des => des.Majors, src => src.Item2.TutorMajors)
            .Map(des => des, src => src.Item2);
    }
}