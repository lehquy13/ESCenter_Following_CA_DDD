using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.Courses;
using FluentValidation;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Users.Learners;

public class LearnerForCreateUpdateDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;
    public string Avatar { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner";
    public bool IsEmailConfirmed { get; set; } = false;
}

public class LearnerForCreateUpdateDtoValidator : AbstractValidator<LearnerForCreateUpdateDto>
{
    public LearnerForCreateUpdateDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(dto => dto.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(dto => dto.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(gender => gender is "Male" or "Female").WithMessage("Invalid gender value.");

        RuleFor(dto => dto.BirthYear)
            .InclusiveBetween(1900, DateTime.UtcNow.Year).WithMessage("Birth year must be between 1900 and current year.");

        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(dto => dto.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .WithMessage("Invalid phone number format.");

        RuleFor(dto => dto.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(dto => dto.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

        RuleFor(dto => dto.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(dto => dto.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => role != Role.SuperAdmin.ToString()).WithMessage("Invalid role value.");

        RuleFor(dto => dto.IsEmailConfirmed)
            .NotNull().WithMessage("Email confirmation status is required.");
    }
}

public class LearnerForCreateUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LearnerForCreateUpdateDto, Customer>();

        config.NewConfig<Customer, LearnerForCreateUpdateDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(des => des.Gender, src => src.Gender)
            .Map(des => des.FirstName, src => src.FirstName)
            .Map(des => des.LastName, src => src.LastName)
            .Map(des => des.BirthYear, src => src.BirthYear)
            .Map(des => des.City, src => src.Address.City)
            .Map(des => des.Country, src => src.Address.Country)
            .Map(des => des.Description, src => src.Description)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.Role, src => src.Role.ToString())
            .Map(des => des.PhoneNumber, src => src.PhoneNumber)
            .IgnoreNonMapped(true);
    }
}