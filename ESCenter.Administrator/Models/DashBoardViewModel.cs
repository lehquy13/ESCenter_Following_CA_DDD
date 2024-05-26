using System.Collections;
using ESCenter.Admin.Application.Contracts.Notifications;
using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.ServiceImpls.DashBoards;

namespace ESCenter.Administrator.Models;

public class DashBoardViewModel
{
    public TotalValueModel<MetricObject> TutorTotalValueModel { get; init; } = new();
    public TotalValueModel<MetricObject> StudentTotalValueModel { get; init; } = new();
    public TotalValueModel<MetricObject> ClassTotalValueModel { get; init; } = new();
    public object? ChartWeekData { get; init; }
    public object? DonutSeries { get; init; }
    public object? DonutLabels { get; init; }
    public object? Xaxis { get; init; }
    public AreaChartViewModel AreaChartViewModel { get; init; } = new();
    public List<NotificationDto> NotificationDtos { get; init; } = new();
    public IEnumerable<TutorRequestForListDto> TutorRequests { get; set; } = [];
}

public class PieChartViewModel
{
    public object? Series { get; init; }
    public object? Labels { get; init; }

    public string ByTime = Domain.Shared.Courses.ByTime.Today;
}

public class AreaChartViewModel
{
    public string TotalRevenue = "Total Revenues";
    public string Refunded = "Refunded";
    public string Incoming = "Incomings";
    public string? TotalRevenueSeries { get; init; }
    public string? RefundedSeries { get; init; }
    public string? IncomingSeries { get; init; }
    public string? Dates { get; init; }

    public string ByTime = Domain.Shared.Courses.ByTime.Today;
}