using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Users;

public interface IUserRepository : IRepository<User, IdentityGuid>
{
    Task<List<User>> GetLearners();
    Task<List<User>> GetTutors();
}