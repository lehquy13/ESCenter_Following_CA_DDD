using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery() : IQueryRequest<UserProfileDto>, IAuthorizationRequest;

public class UserProfileDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int BirthYear { get; set; } = 1960;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner"; // This currently is not mapped
    public string Avatar { get; set; } = string.Empty;
}

public class LearnerForProfileDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, UserProfileDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.City, src => src.Address.City)
            .Map(dest => dest.Avatar, src => src.Avatar)
            .Map(dest => dest.Country, src => src.Address.Country)
            .Map(dest => dest.Role, src => src.Role.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<UserProfileDto, Customer>()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.BirthYear, src => src.BirthYear)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            //.Map(dest => dest.Email, src => src.Email) TODO: we may change here, bc the profile may allow us to change email, and if we do change it, then change the identityUser too 
            .Map(dest => dest.Address, src => Address.Create(src.City, src.Country))
            .Map(dest => dest.Gender, src => src.Gender)
            .IgnoreNonMapped(true);
    }
}