using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Domain.Aggregates.Tutors.DomainEvents;

public record ChangeVerificationRequestCreatedDomainEvent(Tutor Tutor, int ChangeVerificationRequest) : IDomainEvent;

public class ChangeVerificationRequestCreatedDomainEventHandler(
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<ChangeVerificationRequestCreatedDomainEvent>
{
    public Task Handle(ChangeVerificationRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message =
            $"Tutor {notification.Tutor.Id.Value} has created a change verification with {notification.ChangeVerificationRequest} changes";

        notificationRepository.InsertAsync(Notification.Create(
            message,
            notification.Tutor.Id.Value.ToString(),
            NotificationEnum.Tutor
        ), cancellationToken);

        unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Task.CompletedTask;
    }
}