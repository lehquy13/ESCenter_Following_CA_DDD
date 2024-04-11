using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices.Interfaces;

public interface ITutorDomainService : IDomainService
{
    Task<Tutor> CreateTutorWithEmptyVerificationAsync(
        CustomerId userId,
        AcademicLevel academicLevel,
        string university,
        IEnumerable<int> majors,
        bool isVerified
    );

    Task<Result> UpdateTutorMajorsAsync(TutorId tutorId, IEnumerable<int> majorIds);
}