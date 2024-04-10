using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Users;

public interface ICustomerRepository : IRepository<Customer, CustomerId>
{
    Task<List<Customer>> GetLearners();
    Task<List<Customer>> GetTutors();
    Task<string?> GetTutorEmail(TutorId tutorId);
}