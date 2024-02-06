using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Shared.NotificationConsts;

namespace ESCenter.Application.Contracts.Notifications;

public class NotificationDto : BasicAuditedEntityDto<int>
{
    public string Message { get; set; } = string.Empty;
    public int ObjectId { get; set; }
    public string DetailPath { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string NotificationType { get; set; } = NotificationEnum.Unknown.ToString();


}
