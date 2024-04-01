using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class CustomerRepository(AppDbContext appDbContext, IAppLogger<CustomerRepository> appLogger)
    : RepositoryImpl<Customer, CustomerId>(appDbContext,
        appLogger), ICustomerRepository
{
    public async Task<List<Customer>> GetLearners()
    {
        var users = await AppDbContext.Customers
            .AsNoTracking()
            .Where(o => o.Role == Role.Learner &&
                        o.IsDeleted == false)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync();

        return users;
    }

    public async Task<List<Customer>> GetTutorsByIds(IEnumerable<TutorId> tutorIds)
    {
        var users = await AppDbContext.Customers
            .AsNoTracking()
            .Join(AppDbContext.Tutors,
                user => user.Id,
                tutor => tutor.CustomerId,
                (user, tutor) => new { user, tutor })
            .Where(o => o.user.Role == Role.Tutor
                        && tutorIds.Contains(o.tutor.Id)
                        && o.user.IsDeleted == false)
            .Select(x => x.user)
            .ToListAsync();

        return users;
    }

    public async Task<List<Customer>> GetTutors()
    {
        var users = await AppDbContext.Customers
            .Where(o => o.Role == Role.Tutor &&
                        o.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        return users;
    }

    public async Task<Customer?> GetTutor(TutorId tutorId)
    {
        var tutor = await AppDbContext.Customers
            .Join(AppDbContext.Tutors,
                user => user.Id,
                tutor => tutor.CustomerId,
                (user, tutor) => user)
            .FirstOrDefaultAsync();

        return tutor;
    }

    public Task<Customer?> GetUserByContact(string contact)
    {
        return AppDbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PhoneNumber == contact);
    }
}