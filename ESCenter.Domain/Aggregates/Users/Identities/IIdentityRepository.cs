using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainModels.Users;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Users.Identities;

public interface IIdentityRepository : IRepository<IdentityUser, IdentityGuid>
{
    Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);
    Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task<IdentityRole?> GetRolesAsync(IdentityGuid userId, CancellationToken cancellationToken = default);
    Task<IdentityUser?> CheckExistingAccount(string email, string userName);
    Task<UserProfileDomainModel?> GetUserProfileAsync(IdentityGuid id);
    Task<IdentityUser?> ExistenceCheck(IdentityGuid id);
}