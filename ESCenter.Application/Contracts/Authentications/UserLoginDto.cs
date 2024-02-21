using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using Mapster;

namespace ESCenter.Application.Contracts.Authentications;

public class UserLoginDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}

public class UserLoginDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(User, IdentityUser, string), UserLoginDto>()
            .Map(des => des, src => src.Item1.GetFullName())
            .Map(des => des.Image, src => src.Item1.Avatar)
            .Map(des => des.Email, src => src.Item1.Email)
            .Map(des => des.Role, src => src.Item3);
        
        
    }
}