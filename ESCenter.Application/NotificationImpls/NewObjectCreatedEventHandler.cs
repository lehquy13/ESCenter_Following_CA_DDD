using ESCenter.Domain.Aggregates.Notifications;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.NotificationImpls;

internal class NewObjectCreatedEventHandler(
    IRepository<Notification, int> notificationRepository,
    IAppLogger<NewObjectCreatedEventHandler> logger,
    IUnitOfWork unitOfWork)
    : INotificationHandler<NewObjectCreatedEvent>
{
    public async Task Handle(NewObjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new notification...");

        var entityToCreate = Notification.Create(
            notification.Message, (string)notification.ObjectId, notification.NotificationEnum);


        await notificationRepository.InsertAsync(entityToCreate, cancellationToken);

        if (await unitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            logger.LogError("Fail to add new notification");
            return;
        }

        logger.LogInformation("Created new notification");
    }
}