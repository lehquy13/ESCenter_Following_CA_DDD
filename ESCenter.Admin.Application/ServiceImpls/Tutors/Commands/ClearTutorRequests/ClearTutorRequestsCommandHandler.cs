using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.ClearTutorRequests;

public class ClearTutorRequestsCommandHandler(
    ITutorRequestRepository tutorRequestRepository,
    IUnitOfWork unitOfWork, 
    IAppLogger<ClearTutorRequestsCommandHandler> logger)
    : CommandHandlerBase<ClearTutorRequestsCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ClearTutorRequestsCommand command, CancellationToken cancellationToken)
    {
        await tutorRequestRepository.ClearTutorRequests(CustomerId.Create(command.TutorId), cancellationToken);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail("Cannot delete the request");
        }

        return Result.Success();
    }
}