﻿using ESCenter.Admin.Application.Contracts.Charts;
using ESCenter.Admin.Application.Contracts.Notifications;
using ESCenter.Admin.Application.Contracts.Users.Learners;

namespace ESCenter.Admin.Application.DashBoards;

public interface IDashboardServices
{
    Task<AreaChartData> GetAreaChartData(string byTime = "");
    Task<DonutChartData> GetDonutChartData(string byTime = "");
    Task<LineChartData> GetLineChartData(string byTime = "");
    Task<List<NotificationDto>> GetNotification();
    Task<List<MetricObject>> GetTutorsMetrics();
    Task<List<MetricObject>>  GetCoursesMetrics();
    Task<List<MetricObject>>  GetLearnersMetrics();
    Task<IEnumerable<TutorRequestForListDto>> GetLatestTutorRequests();
}

public class MetricObject
{
    public object Id { get; set; } = null!;
    public DateTime CreationTime { get; set; }
}