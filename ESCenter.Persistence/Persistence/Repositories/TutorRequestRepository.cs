using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class TutorRequestRepository(
    AppDbContext appDbContext,
    IAppLogger<RepositoryImpl<TutorRequest, TutorRequestId>> logger)
    : RepositoryImpl<TutorRequest, TutorRequestId>(appDbContext, logger), ITutorRequestRepository
{
    public Task ClearTutorRequests(CustomerId tutorId, CancellationToken cancellationToken = default)
    {
        var tutorRequests = AppDbContext.TutorRequests.Where(x => x.TutorId == tutorId);
        AppDbContext.TutorRequests.RemoveRange(tutorRequests);

        return Task.CompletedTask;
    }
}