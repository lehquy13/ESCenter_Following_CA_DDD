using ESCenter.Domain.Aggregates.Subjects;
using FluentValidation;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public class SubjectDto : EntityDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class SubjectDtoValidator : AbstractValidator<SubjectDto>
{
    public SubjectDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
    }
}

public class SubjectDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Subject, SubjectDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .IgnoreNonMapped(true);
        config.NewConfig<SubjectDto, Subject>();
    }
}