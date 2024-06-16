using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.TutorRequests.Commands.MarkRequestAsDone;

public class MarkRequestAsDoneCommandHandler(
    ITutorRequestRepository tutorRequestRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger
) : CommandHandlerBase<MarkRequestAsDoneCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(MarkRequestAsDoneCommand command, CancellationToken cancellationToken)
    {
        var request = await tutorRequestRepository.GetAsync(
            TutorRequestId.Create(command.RequestId), cancellationToken);

        if (request is null)
        {
            return Result.Fail(TutorRequestAppServiceError.RequestNotFound);
        }

        request.Done();

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}