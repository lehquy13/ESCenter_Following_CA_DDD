using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Application.Contracts.Users.Learners;

public class LearnerForProfileDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Avatar { get; set; } =
        @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner"; // This currently is not mapped
}

public class LearnerForProfileDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, LearnerForProfileDto>();
    }
}