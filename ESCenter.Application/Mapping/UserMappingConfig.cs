using ESCenter.Application.Contract.Users.BasicUsers;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using Mapster;

namespace ESCenter.Application.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(IdentityUser, User), UserForDetailDto>()
            .Map(des => des.Id, src => src.Item1.Id.Value)
            .Map(des => des.Gender, src => src.Item2.Gender)
            .Map(des => des.Role, src => src.Item1.IdentityRole.Name)
            .Map(des => des.Email, src => src.Item1.Email)
            .Map(des => des.PhoneNumber, src => src.Item1.PhoneNumber)
            .Map(des => des, src => src.Item2);
    }
}
