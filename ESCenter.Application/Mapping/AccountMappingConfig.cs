using ESCenter.Application.Contract.Users.Learners;
using ESCenter.Domain.Aggregates.Users;
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
    }
}