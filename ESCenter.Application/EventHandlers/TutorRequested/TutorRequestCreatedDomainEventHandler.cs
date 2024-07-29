using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.TutorRequests.DomainEvents;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.EventHandlers.TutorRequested;

public class TutorRequestCreatedDomainEventHandler(
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<TutorRequestCreatedDomainEvent>
{
    public async Task Handle(TutorRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = $"New tutoring request from {notification.TutorRequest.LearnerId}";
        var notificationObject = Notification.Create(
            message,
            notification.TutorRequest.Id.Value.ToString(),
            NotificationEnum.TutorRequest
        );

        await notificationRepository.InsertAsync(notificationObject, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}