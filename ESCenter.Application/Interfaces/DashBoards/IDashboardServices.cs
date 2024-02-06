using ESCenter.Application.Contracts.Charts;
using ESCenter.Application.Contracts.Notifications;
using Matt.ResultObject;

namespace ESCenter.Application.Interfaces.DashBoards;

public interface IDashboardServices
{
    Task<Result<AreaChartData>> GetAreaChartData(GetAreaChartDataQuery getAreaChartDataQuery);
    Task<Result<DonutChartData>> GetDonutChartData(GetDonutChartDataQuery getDonutChartDataQuery);
    Task<Result<LineChartData>> GetLineChartData(GetLineChartDataQuery getLineChartDataQuery);
    Task<Result<List<NotificationDto>>> GetNotification(GetNotificationQuery getNotificationQuery);
}