using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Domain.Aggregates.Courses.DomainEvents;

public record CourseRequestedDomainEvent(Course Course) : IDomainEvent;

public class CourseRequestedDomainEventHandler(
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<CourseRequestedDomainEvent>
{
    public async Task Handle(CourseRequestedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = $"New course request to course {TitleFormat(notification.Course.Title)}" +
                      $"at {DateTime.Now.ToLongDateString()}";

        var notificationObject = Notification.Create(
            message,
            notification.Course.Id.Value.ToString(),
            NotificationEnum.Course
        );

        await notificationRepository.InsertAsync(notificationObject, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private string TitleFormat(string title)
    {
        return title.Length > 12 ? title[..12] + "..." : title;
    }
}