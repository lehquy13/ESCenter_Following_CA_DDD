using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Notifications;

public class Notification : AuditedAggregateRoot<int>
{
    public string Message { get; private set; } = null!;
    public string ObjectId { get; private set; } = null!;
    public bool IsRead { get; private set; }
    public NotificationEnum NotificationType { get; private set; } = NotificationEnum.Unknown;

    private Notification()
    {
    }

    public static Notification Create(
        string message,
        string objectId,
        NotificationEnum notificationType)
    {
        return new Notification()
        {
            Message = message,
            ObjectId = objectId,
            NotificationType = notificationType
        };
    }
}