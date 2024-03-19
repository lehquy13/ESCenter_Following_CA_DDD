using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
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
    public string Role { get; set; } = Domain.Shared.Courses.UserRole.Learner.ToString();

}

public class UserForDetailDtoMappingConfig : IRegister
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
