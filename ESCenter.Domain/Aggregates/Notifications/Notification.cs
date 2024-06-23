using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Notifications;

public class Notification : AuditedAggregateRoot<int>
{
    public string Message { get; private set; } = null!;
    public Guid? From { get; private set; }
    public Guid To { get; private set; }
    public bool IsRead { get; private set; }
    public string ObjectId { get; private set; } = null!;
    public NotificationEnum NotificationType { get; private set; } = NotificationEnum.Unknown;

    private Notification()
    {
    }

    public static Notification Create(
        string message,
        string objectId,
        NotificationEnum notificationType,
        Guid? from = null,
        Guid? to = null)
    {
        return new Notification()
        {
            Message = message,
            ObjectId = objectId,
            NotificationType = notificationType,
            From = from,
            To = to ?? Guid.Empty
        };
    }
}