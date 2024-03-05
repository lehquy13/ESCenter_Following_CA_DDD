using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.Contracts.Commons;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Profiles;

public class UserProfileDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner"; // This currently is not mapped

    public string Avatar { get; set; } =
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
}

public class LearnerForProfileDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserProfileDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.City, src => src.Address.City)
            .Map(dest => dest.Country, src => src.Address.Country)
            .Map(dest => dest.Role, src => src.Role.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<UserProfileDto, User>()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.BirthYear, src => src.BirthYear)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            //.Map(dest => dest.Email, src => src.Email) TODO: we may change here, bc the profile may allow us to change email, and if we do change it, then change the identityUser too 
            .Map(dest => dest.Address, src => Address.Create(src.City, src.Country))
            .Map(dest => dest.Gender, src => src.Gender.ToEnum<Gender>())
            .IgnoreNonMapped(true);
    }
}