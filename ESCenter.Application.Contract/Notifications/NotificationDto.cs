using ESCenter.Application.Contract.Commons.Primitives.Auditings;
using ESCenter.Domain.Shared.NotificationConsts;

namespace ESCenter.Application.Contract.Notifications;

public class NotificationDto : FullAuditedAggregateRootDto<int>
{
    public string Message { get; set; } = string.Empty;
    public int ObjectId { get; set; }
    public string DetailPath { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public NotificationEnum NotificationType { get; set; } = NotificationEnum.Unknown;


}
