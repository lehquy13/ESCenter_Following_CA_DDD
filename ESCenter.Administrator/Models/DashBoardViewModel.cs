using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Application.Contract.Notifications;
using ESCenter.Application.Contract.Users.BasicUsers;
using ESCenter.Application.Contract.Users.Tutors;

namespace ESCenter.Administrator.Models;

public class DashBoardViewModel
{
    public TotalValueModel<TutorForListDto> TutorTotalValueModel{ get; set; } = new();
    public TotalValueModel<UserForListDto> StudentTotalValueModel{ get; set; } = new();

    public TotalValueModel<CourseForListDto> ClassTotalValueModel{ get; set; } = new();

    public object? ChartWeekData { get; set; }
    public object? PieWeekData1 { get; set; }
    public object? PieWeekData2 { get; set; }
    public object? DatesWeekData { get; set; }
    public AreaChartViewModel AreaChartViewModel { get; set; } = new AreaChartViewModel();
    
    public List<SubjectDto> SubjectDtos { get; set; } = new();
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