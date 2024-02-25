using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Shared.NotificationConsts;
using Mapster;

namespace ESCenter.Application.Contracts.Notifications;

public class NotificationDto : BasicAuditedEntityDto<int>
{
    public string Message { get; set; } = string.Empty;
    public object ObjectId { get; set; } = null!;
    public string DetailPath { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string NotificationType { get; set; } = NotificationEnum.Unknown.ToString();
}

public class NotificationDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Notification, NotificationDto>()
            .Map(des => des.DetailPath, src =>
                src.NotificationType == NotificationEnum.CourseRequest
                    ? $"/{src.NotificationType.ToString()}/Edit/{src.ObjectId}"
                    : $"/{src.NotificationType.ToString()}/Detail?id={src.ObjectId}")
            .Map(des => des, src => src);
    }
}