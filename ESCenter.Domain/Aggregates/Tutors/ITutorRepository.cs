using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Tutors;

public interface ITutorRepository : IRepository<Tutor, TutorId>
{
    Task<List<Tutor>> GetPopularTutors();
    Task<Tutor?> GetTutorByUserId(CustomerId userId, CancellationToken cancellationToken = default);
    Task RemoveChangeVerification(ChangeVerificationRequestId id);
    Task<List<TutorId>> GetTutorsBySubjectId(SubjectId create, CancellationToken cancellationToken);
}