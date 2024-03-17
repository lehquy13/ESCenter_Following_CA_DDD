using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using FluentValidation;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Admin.Application.Contracts.Users.Tutors;


public class TutorProfileCreateDto
{
    public string AcademicLevel { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public List<int> MajorIds { get; set; } = new();
    public bool IsVerified { get; set; } 
}

public class TutorProfileCreateDtoValidator : AbstractValidator<TutorProfileCreateDto>
{
    public TutorProfileCreateDtoValidator()
    {
        RuleFor(dto => dto.AcademicLevel)
            .NotEmpty().WithMessage("Academic level is required.")
            .MaximumLength(100).WithMessage("Academic level must not exceed 100 characters.");

        RuleFor(dto => dto.University)
            .NotEmpty().WithMessage("University is required.")
            .MaximumLength(100).WithMessage("University name must not exceed 100 characters.");

        RuleFor(dto => dto.MajorIds)
            .NotEmpty().WithMessage("At least one major must be selected.")
            .Must(ids => ids.All(id => id > 0)).WithMessage("Invalid major ID(s).");

        RuleFor(dto => dto.IsVerified)
            .NotNull().WithMessage("Verification status is required.");
    }
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