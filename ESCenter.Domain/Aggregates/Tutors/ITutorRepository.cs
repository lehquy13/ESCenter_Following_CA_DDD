using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Tutors;

public interface ITutorRepository : IRepository<Tutor, TutorId>
{
    Task<List<Tutor>> GetPopularTutors();
    Task<Tutor?> GetTutorByUserId(IdentityGuid userId);
}

