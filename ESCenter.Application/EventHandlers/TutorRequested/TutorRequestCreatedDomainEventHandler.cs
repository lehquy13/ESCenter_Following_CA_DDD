using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.TutorRequests.DomainEvents;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.EventHandlers.TutorRequested;

public class TutorRequestCreatedDomainEventHandler(
    IRepository<Notification, int> notificationRepository
) : INotificationHandler<TutorRequestCreatedDomainEvent>
{
    public Task Handle(TutorRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = $"New tutor request from {notification.TutorRequest.LearnerId}";
        var notificationObject = Notification.Create(
            message,
            notification.TutorRequest.Id.Value.ToString(),
            NotificationEnum.TutorRequest
        );

        notificationRepository.InsertAsync(notificationObject, cancellationToken);

        return Task.CompletedTask;
    }
}