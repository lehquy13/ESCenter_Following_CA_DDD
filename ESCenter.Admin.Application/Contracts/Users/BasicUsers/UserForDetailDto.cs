using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Users.BasicUsers;

public class UserForDetailDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = Domain.Shared.Courses.GenderEnum.Male;
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = Domain.Shared.Courses.Role.Learner.ToString();
}

public class UserForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, UserForDetailDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des.Gender, src => src.Gender)
            .Map(des => des.Role, src => src.Role.ToString())
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.PhoneNumber, src => src.PhoneNumber)
            .Map(des => des, src => src);
    }
}