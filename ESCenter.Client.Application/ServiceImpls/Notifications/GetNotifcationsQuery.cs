using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using Notification = ESCenter.Domain.Aggregates.Notifications.Notification;

namespace ESCenter.Client.Application.ServiceImpls.Notifications;

public record GetNotificationsQuery : IQueryRequest<List<NotificationDto>>;

public class NotificationDto : BasicAuditedEntityDto<int>
{
    public string Message { get; set; } = string.Empty;
    public object ObjectId { get; set; } = null!;
    public string DetailPath { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string NotificationType { get; set; } = NotificationEnum.Unknown.ToString();
    public string TimeAgo => AsTimeAgo(CreationTime);

    private static string AsTimeAgo(DateTime dateTime)
    {
        var timeSpan = DateTime.Now.Subtract(dateTime);

        return timeSpan.TotalSeconds switch
        {
            <= 60 => $"{timeSpan.Seconds} secs",

            _ => timeSpan.TotalMinutes switch
            {
                <= 1 => "a minute",
                < 60 => $"{timeSpan.Minutes} minutes",
                _ => timeSpan.TotalHours switch
                {
                    <= 1 => "an hour",
                    < 24 => $"{timeSpan.Hours} hours",
                    _ => timeSpan.TotalDays switch
                    {
                        <= 1 => "yesterday",
                        <= 30 => $"{timeSpan.Days} days",

                        <= 60 => "a month",
                        < 365 => $"{timeSpan.Days / 30} months",

                        <= 365 * 2 => "a year",
                        _ => $"{timeSpan.Days / 365} years"
                    }
                }
            }
        };
    }
}

public class GetNotificationsQueryHandler(
    IReadOnlyRepository<Notification, int> notificationRepository,
    ICurrentUserService currentUserService,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetNotificationsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetNotificationsQuery, List<NotificationDto>>(logger, mapper)
{
    public override async Task<Result<List<NotificationDto>>> Handle(GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        // Create a dateListRange
        var notificationsQueryable = notificationRepository
            .GetAll()
            .Where(x => x.IsRead == false
                        && x.To == currentUserService.UserId)
            .OrderByDescending(x => x.CreationTime);

        var notifications = await asyncQueryableExecutor.ToListAsync(notificationsQueryable, false, cancellationToken);

        return (from notification in notifications
            let detailPath = notification.NotificationType switch
            {
                NotificationEnum.CourseRequest or NotificationEnum.Course => $"course/{notification.ObjectId}",
                NotificationEnum.TutorRequest => $"tutor-request/edit/{notification.ObjectId}",
                NotificationEnum.Tutor => $"tutor/{notification.ObjectId}",
                NotificationEnum.Learner => $"user/{notification.ObjectId}",
                _ => ""
            }
            select new NotificationDto
            {
                CreationTime = notification.CreationTime,
                Id = notification.Id,
                IsRead = notification.IsRead,
                Message = notification.Message,
                NotificationType = notification.NotificationType.ToString(),
                ObjectId = notification.ObjectId,
                DetailPath = detailPath
            }).ToList();
    }
}