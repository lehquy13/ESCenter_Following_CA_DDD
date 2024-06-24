using ESCenter.Admin.Application.Contracts.Commons;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;

namespace ESCenter.Admin.Application.Contracts.Notifications;

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
                < 60 => $"{timeSpan.Minutes} mins",
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