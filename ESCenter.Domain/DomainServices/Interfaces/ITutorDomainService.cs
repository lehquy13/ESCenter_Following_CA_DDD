using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices.Interfaces;

public interface ITutorDomainService : IDomainService
{
    Task<Tutor> CreateTutorWithEmptyVerificationAsync(
        IdentityGuid userId,
        AcademicLevel academicLevel,
        string university,
        IEnumerable<int> majors, 
        bool isVerified
    );
}