using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices;

public class UserDomainService(
    IAppLogger<UserDomainService> logger,
    IUserRepository userRepository,
    IIdentityRepository identityRepository
) : DomainServiceBase(logger), IUserDomainService
{
    private const string DefaultPassword = "1q2w3E*";

    private const string DefaultAvatar =
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public async Task<User> CreateAsync(
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber,
        UserRole role)
    {
        var identityUser = IdentityUser.Create(email, email, DefaultPassword, phoneNumber,
            IdentityRoleId.Create((int)role));

        var user = User.Create(
            identityUser.Id,
            firstName,
            lastName,
            gender,
            birthYear,
            address,
            description,
            avatar,
            email,
            phoneNumber,
            role
        );

        await userRepository.InsertAsync(user);
        await identityRepository.InsertAsync(identityUser);
        return user;
    }
}