using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Application.Contracts.Authentications;

public class UserLoginDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}

public class UserLoginDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, UserLoginDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src.GetFullName())
            .Map(des => des.Avatar, src => src.Avatar)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.Role, src => src.Role.ToString());
    }
}