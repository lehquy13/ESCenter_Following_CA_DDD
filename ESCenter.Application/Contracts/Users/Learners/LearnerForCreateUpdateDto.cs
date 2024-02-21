using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Mapster;

namespace ESCenter.Application.Contracts.Users.Learners;

public class LearnerForCreateUpdateDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;

    public string Image { get; set; } =
        @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner";
    public bool IsEmailConfirmed { get; set; } = false;
}

public class LearnerForCreateUpdateDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LearnerForCreateUpdateDto, User>()
            .Map(dest => dest.Id,
                src => src.Id == Guid.Empty
                    ? IdentityGuid.Create(Guid.Empty)
                    : IdentityGuid.Create(src.Id))
            .Map(des => des.Gender, src => src.Gender)
            .Map(des => des.FirstName, src => src.FirstName)
            .Map(des => des.LastName, src => src.LastName)
            .Map(des => des.BirthYear, src => src.BirthYear)

            .Map(des => des.Address.City, src => src.City)
            .Map(des => des.Address.Country, src => src.Country)
            .Map(des => des.Description, src => src.Description)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.PhoneNumber, src => src.PhoneNumber)
            .IgnoreNonMapped(true);

        config.NewConfig<User, LearnerForCreateUpdateDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(des => des.Gender, src => src.Gender)
            .Map(des => des.FirstName, src => src.FirstName)
            .Map(des => des.LastName, src => src.LastName)
            .Map(des => des.BirthYear, src => src.BirthYear)
            .Map(des => des.City, src => src.Address.City)
            .Map(des => des.Country, src => src.Address.Country)
            .Map(des => des.Description, src => src.Description)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.PhoneNumber, src => src.PhoneNumber)
            .IgnoreNonMapped(true);
    }
}