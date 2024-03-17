using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class UserRepository(AppDbContext appDbContext, IAppLogger<UserRepository> appLogger)
    : RepositoryImpl<User, IdentityGuid>(appDbContext,
        appLogger), IUserRepository
{
    public async Task<List<User>> GetLearners()
    {
        var users = await AppDbContext.Users
            .AsNoTracking()
            .Where(o => o.Role == UserRole.Learner &&
                        o.IsDeleted == false)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync();

        return users;
    }

    public async Task<List<User>> GetTutorsByIds(IEnumerable<TutorId> tutorIds)
    {
        var users = await AppDbContext.Users
            .AsNoTracking()
            .Join(AppDbContext.Tutors,
                user => user.Id,
                tutor => tutor.UserId,
                (user, tutor) => new { user, tutor })
            .Where(o => o.user.Role == UserRole.Tutor
                        && tutorIds.Contains(o.tutor.Id)
                        && o.user.IsDeleted == false)
            .Select(x => x.user)
            .ToListAsync();

        return users;
    }

    public async Task<List<User>> GetTutors()
    {
        var users = await AppDbContext.Users
            .Where(o => o.Role == UserRole.Tutor &&
                        o.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        return users;
    }

    public async Task<User?> GetTutor(TutorId tutorId)
    {
        var tutor = await AppDbContext.Users
            .Join(AppDbContext.Tutors,
                user => user.Id,
                tutor => tutor.UserId,
                (user, tutor) => user)
            .FirstOrDefaultAsync();

        return tutor;
    }

    public Task<User?> GetUserByContact(string contact)
    {
        return AppDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PhoneNumber == contact);
    }
}