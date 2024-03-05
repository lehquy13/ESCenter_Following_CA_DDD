using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Admin.Application.Contracts.Users.Tutors;

public class TutorCreateDto : EntityDto<Guid>
{
    public LearnerForCreateUpdateDto LearnerForCreateUpdateDto { get; set; } = new();
    public TutorProfileCreateDto TutorProfileCreateDto { get; set; } = new();
}

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