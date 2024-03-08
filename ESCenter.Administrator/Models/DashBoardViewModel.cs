using ESCenter.Admin.Application.Contracts.Notifications;
using ESCenter.Admin.Application.DashBoards;

namespace ESCenter.Administrator.Models;

public class DashBoardViewModel
{
    public TotalValueModel<MetricObject> TutorTotalValueModel{ get; set; } = new();
    public TotalValueModel<MetricObject> StudentTotalValueModel{ get; set; } = new();
    public TotalValueModel<MetricObject> ClassTotalValueModel{ get; set; } = new();
    public object? ChartWeekData { get; set; }
    public object? DonutSeries { get; set; }
    public object? DonutLabels { get; set; }
    public object? Xaxis { get; set; }
    public AreaChartViewModel AreaChartViewModel { get; set; } = new();
    public List<NotificationDto> NotificationDtos { get; set; } = new();
}

public class PieChartViewModel
{
    public object? Series { get; set; } 
    public object? Labels { get; set; }

    public string ByTime = Domain.Shared.Courses.ByTime.Today;
}
public class AreaChartViewModel
{
    public string TotalRevenue = "Total Revenues";
    public string Refunded = "Refunded";
    public string Incoming = "Incomings";
    public string? TotalRevenueSeries { get; set; } 
    public string? RefundedSeries { get; set; } 
    public string? IncomingSeries { get; set; } 
    public string? Dates { get; set; }

    public string ByTime = Domain.Shared.Courses.ByTime.Today;
}