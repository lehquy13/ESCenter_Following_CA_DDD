using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Users.BasicUsers;

public class UserForListDto : BasicAuditedEntityDto<Guid>
{
    //User information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;

    //Account References
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class UserForListDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, UserForListDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);
    }
}