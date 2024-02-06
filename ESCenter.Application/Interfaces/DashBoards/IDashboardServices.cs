using ESCenter.Application.Contract.Charts;
using ESCenter.Application.Contract.Notifications;
using Matt.ResultObject;

namespace ESCenter.Application.Interfaces.DashBoards;

public interface IDashboardServices
{
    Task<Result<AreaChartData>> GetAreaChartData(GetAreaChartDataQuery getAreaChartDataQuery);
    Task<Result<DonutChartData>> GetDonutChartData(GetDonutChartDataQuery getDonutChartDataQuery);
    Task<Result<LineChartData>> GetLineChartData(GetLineChartDataQuery getLineChartDataQuery);
    Task<Result<List<NotificationDto>>> GetNotification(GetNotificationQuery getNotificationQuery);
}