using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainModels.Users;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class IdentityRepository(
    AppDbContext appDbContext, 
    IAppLogger<IdentityRepository> logger)
    : RepositoryImpl<IdentityUser, IdentityGuid>(appDbContext, logger), IIdentityRepository
{
    public async Task<IdentityUser?> GetByIdAsync(IdentityGuid id)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetByIdAsync", ex.Message);
            return null;
        }
    }

    public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.UserName != null && x.UserName.ToUpper() == normalizedUserName,
                    cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "FindByNameAsync", ex.Message);
            return null;
        }
    }

    public async Task<IdentityUser?> FindByEmailAsync(string normalizedEmail,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.Email != null && x.Email.ToUpper() == normalizedEmail, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "FindByEmailAsync", ex.Message);
            return null;
        }
    }

    public async Task<IdentityRole?> GetRolesAsync(IdentityGuid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var identityUser = await AppDbContext.IdentityUsers
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (identityUser is null)
            {
                throw new Exception("User not found.");
            }

            return identityUser.IdentityRole;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetRolesAsync", ex.Message);
            return null;
        }
    }

    public Task<IdentityUser?> CheckExistingAccount(string email, string userName)
    {
        try
        {
            return AppDbContext.IdentityUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email || x.UserName == userName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "CheckExistingAccount", ex.Message);
            throw;
        }
    }

    public async Task<UserProfileDomainModel?> GetUserProfileAsync(IdentityGuid id)
    {
        try
        {
            var queryable =
                from identityUser in AppDbContext.IdentityUsers
                join user in AppDbContext.Users on identityUser.Id equals user.Id
                where identityUser.Id.Equals(id)
                select new UserProfileDomainModel(identityUser, user);

            var userProfile = await queryable.FirstOrDefaultAsync();

            return userProfile;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "CheckExistingAccount", ex.Message);
            throw;
        }
    }

    public Task<IdentityUser?> ExistenceCheck(IdentityGuid id)
    {
        try
        {
            return AppDbContext.IdentityUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == (IdentityGuid)id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "ExistenceCheck", ex.Message);
            throw;
        }
    }
}