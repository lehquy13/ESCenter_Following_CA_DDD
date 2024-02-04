using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainModels.Tutors;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Tutors;

public interface ITutorRepository : IRepository<Tutor, IdentityGuid>
{
    Task<List<Tutor>> GetPopularTutors();
}

