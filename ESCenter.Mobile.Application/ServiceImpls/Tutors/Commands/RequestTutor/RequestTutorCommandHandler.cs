using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public class RequestTutorCommandHandler(
    ITutorDomainService tutorDomainService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestTutorCommandHandler> logger)
    : CommandHandlerBase<RequestTutorCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RequestTutorCommand command, CancellationToken cancellationToken)
    {
        var result = await tutorDomainService.RequestTutor(
            TutorId.Create(command.TutorId),
            CustomerId.Create(currentUserService.UserId),
            command.RequestMessage);

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserError.FailToRequestTutor);
        }

        return Result.Success();
    }
}