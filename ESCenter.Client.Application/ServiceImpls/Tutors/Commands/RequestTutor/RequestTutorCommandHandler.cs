using ESCenter.Application.EventHandlers;
using ESCenter.Domain;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public class RequestTutorCommandHandler(
    IRepository<TutorRequest, TutorRequestId> tutorRequestRepository,
    IPublisher publisher,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestTutorCommandHandler> logger)
    : CommandHandlerBase<RequestTutorCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RequestTutorCommand command, CancellationToken cancellationToken)
    {
        // TODO: we may need to check if the tutor is verified or exists or not
        if (command.TutorRequestForCreateDto.TutorId == command.TutorRequestForCreateDto.LearnerId)
        {
            return Result.Fail(TutorAppServiceError.CantRequestTutorToSelf);
        }

        var tutorRequest = TutorRequest.Create(
            TutorId.Create(command.TutorRequestForCreateDto.TutorId),
            CustomerId.Create(command.TutorRequestForCreateDto.LearnerId),
            command.TutorRequestForCreateDto.RequestMessage);

        await tutorRequestRepository.InsertAsync(tutorRequest, cancellationToken);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserError.FailToRequestTutor);
        }

        var message = "New tutor request to: " + tutorRequest.TutorId + " at " +
                      DateTime.Now.ToLongDateString();

        await publisher.Publish(new NewDomainObjectCreatedEvent(tutorRequest.TutorId.Value.ToString(), message,
            NotificationEnum.TutorRequest), cancellationToken);

        return Result.Success();
    }
}