using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorCreateUpdateDto : EntityDto<Guid>
{
    public LearnerForCreateUpdateDto LearnerForCreateUpdateDto { get; set; } = null!;
    public TutorProfileCreateDto TutorProfileCreateDto { get; set; } = null!;
}

public record TutorProfileCreateDto(string AcademicLevel, string University, List<int> Majors);

public class TutorCreateUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TutorProfileCreateDto, Tutor>()
            .Map(dest => dest.AcademicLevel, src => src.AcademicLevel.ToEnum<AcademicLevel>())
            .Map(dest => dest.University, src => src.University);
    }
} 