using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.TutorRequests;

public interface ITutorRequestRepository : IRepository<TutorRequest, TutorRequestId>
{
    Task ClearTutorRequests(CustomerId tutorId, CancellationToken cancellationToken = default);
}