using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using Mapster;

namespace ESCenter.Application.Mapping;

public class AccountMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LearnerForCreateUpdateDto, User>()
            .Map(des => des.Gender, src => src.Gender)
            .Map(des => des.FirstName, src => src.FirstName)
            .Map(des => des.LastName, src => src.LastName)
            .Map(des => des.BirthYear, src => src.BirthYear)
            .Map(des => des.Address, src => src.Address)
            .Map(des => des.Description, src => src.Description)
            // Currently not support for changing email and phone number
            //.Map(des => des.Email, src => src.Email) 
            //.Map(des => des.PhoneNumber, src => src.PhoneNumber)
            .IgnoreNonMapped(true);

        config.NewConfig<(User, IdentityUser, string), UserLoginDto>()
            .Map(des => des, src => src.Item1.GetFullName())
            .Map(des => des.Image, src => src.Item1.Avatar)
            .Map(des => des.Email, src => src.Item1.Email)
            .Map(des => des.Role, src => src.Item3);
    }
}