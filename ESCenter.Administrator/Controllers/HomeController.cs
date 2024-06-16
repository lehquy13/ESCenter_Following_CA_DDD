using System.Diagnostics;
using ESCenter.Admin.Application.ServiceImpls.DashBoards;
using ESCenter.Administrator.Models;
using ESCenter.Administrator.Utilities;
using ESCenter.Domain.Shared.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ESCenter.Administrator.Controllers;

[Route("admin/[controller]")]
[Authorize(Policy = "RequireAdministratorRole")]
public class HomeController(
    ILogger<HomeController> logger,
    IDashboardServices dashboardServices) : Controller
{
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var tutorDtos = await dashboardServices.GetTutorsMetrics();
        var classDtos = await dashboardServices.GetCoursesMetrics();
        var learner = await dashboardServices.GetLearnersMetrics();

        var date = GetByTime(DateTime.Now, ByTime.Month);
        var courseCountByTime = classDtos.Count(x => x.CreationTime >= date);
        var tutorCountByTime = tutorDtos.Count(x => x.CreationTime >= date);
        var learnerCountByTime = learner.Count(x => x.CreationTime >= date);

        date = GetByTime(date, ByTime.Month);
        var courseCountByLastTime = classDtos.Count(x => x.CreationTime >= date);
        var tutorCountByLastTime = tutorDtos.Count(x => x.CreationTime >= date);
        var learnerCountByLastTime = learner.Count(x => x.CreationTime >= date);

        logger.LogDebug("On getting lineChartData, donutChartData...");
        //var lineChartData = await GetLineChart(ByTime.Week);
        var donutChartData = await GetDonutChart(ByTime.Month);
        var areaListData = await AreaChartDataCalculate(ByTime.Month);
        var lineChartData = await GetLineChart(ByTime.Month);

        return View(
            new DashBoardViewModel
            {
                StudentTotalValueModel = new TotalValueModel<MetricObject>()
                {
                    Models = learner,
                    IsIncrease = learnerCountByTime > learnerCountByLastTime,
                    IncreasePercentage = Math.Round(Math.Abs(learnerCountByTime - learnerCountByLastTime) * 1.0 /
                        learnerCountByLastTime * 100, 2),
                },
                ClassTotalValueModel = new TotalValueModel<MetricObject>()
                {
                    Models = classDtos,
                    IsIncrease = courseCountByTime > courseCountByLastTime,
                    IncreasePercentage =
                        Math.Round(Math.Abs(courseCountByTime - courseCountByLastTime) * 1.0 /
                        courseCountByLastTime * 100, 2),
                },
                TutorTotalValueModel = new TotalValueModel<MetricObject>()
                {
                    Models = tutorDtos,
                    IsIncrease = tutorCountByTime > tutorCountByLastTime,
                    IncreasePercentage = Math.Round(
                            Math.Abs(tutorCountByTime - tutorCountByLastTime) * 1.0 /
                        tutorCountByLastTime * 100, 2),
                },

                // Chart
                DonutSeries = donutChartData.Series,
                DonutLabels = donutChartData.Labels,
                ChartWeekData = lineChartData.Item1,
                Xaxis = lineChartData.Item2,
                //Incomes Chart
                AreaChartViewModel = new AreaChartViewModel()
                {
                    Dates = areaListData.ElementAt(0),
                    TotalRevenueSeries = areaListData.ElementAt(1),
                    RefundedSeries = areaListData.ElementAt(2),
                    IncomingSeries = areaListData.ElementAt(3),
                    ByTime = ByTime.Week
                },
                NotificationDtos = await dashboardServices.GetNotification(),
                TutorRequests = await dashboardServices.GetLatestTutorRequests()
            }
        );
    }

    [HttpGet]
    [Route("filter-line-chart/{byTime?}")]
    public async Task<IActionResult> FilterLineChart(string byTime = ByTime.Month)
    {
        var a = await GetLineChart(byTime);

        return Json(new
        {
            ChartWeekData = a.Item1,
            DatesWeekData = a.Item1,
        });
    }


    [HttpGet]
    [Route("filter-pie-chart/{byTime?}")]
    public async Task<IActionResult> FilterPieChart(string byTime = "")
    {
        logger.LogDebug("filterPieChart's running! On getting DonutChartData...");

        return Helper.RenderRazorViewToString(this, "_PieChart",
            await GetDonutChart(byTime));
    }


    [HttpGet]
    [Route("filter-area-chart/{byTime?}")]
    public async Task<IActionResult> FilterAreaChart(string byTime = "")
    {
        var listData = await AreaChartDataCalculate(ByTime.Month);
        return Helper.RenderRazorViewToString(this, "_AreaChart",
            new AreaChartViewModel()
            {
                Dates = listData.ElementAt(1),
                TotalRevenueSeries = listData.ElementAt(1),
                RefundedSeries = listData.ElementAt(3),
                IncomingSeries = listData.ElementAt(4),
                ByTime = byTime
            });
    }

    [HttpGet]
    [Route("filter-total-classes/{byTime?}")]
    public async Task<IActionResult> FilterTotalClasses(string byTime = "")
    {
        logger.LogDebug("Index's running! On getting classDtos...");
        var classDtos = await dashboardServices.GetCoursesMetrics();

        var date = GetByTime(DateTime.Now, byTime);
        var result1 = classDtos.Where(x => x.CreationTime >= date).ToList();
        var date2 = GetByTime(date, byTime);
        var result2 = classDtos.Where(x => x.CreationTime >= date2 && x.CreationTime <= date).ToList();

        return Helper.RenderRazorViewToString(this, "_TotalClasses", new TotalValueModel<MetricObject>
        {
            Models = result1,
            IsIncrease = result1.Count > result2.Count,
            IncreasePercentage = (Math.Abs(result1.Count - result2.Count) * 1.0 / result2.Count) * 100,
            Time = byTime
        });
    }

    [HttpGet]
    [Route("FilterTotalTutors/{byTime?}")]
    public async Task<IActionResult> FilterTotalTutors(string? byTime)
    {
        logger.LogDebug("Index's running! On getting tutorDtos...");
        var tutorDtos = await dashboardServices.GetTutorsMetrics();

        logger.LogDebug("Got tutorDtos!");
        var date = GetByTime(DateTime.Now, byTime);
        var result1 = tutorDtos.Where(x => x.CreationTime >= date).ToList();
        var date2 = GetByTime(date, byTime);

        var result2 = tutorDtos.Where(x => x.CreationTime >= date2 && x.CreationTime <= date).ToList();

        return Helper.RenderRazorViewToString(this, "_TotalTutors", new TotalValueModel<MetricObject>
        {
            Models = result1,
            IsIncrease = result1.Count > result2.Count,
            IncreasePercentage = Math.Abs(result1.Count - result2.Count) * 1.0 / result2.Count * 100,
            Time = byTime ?? "Today"
        });
    }

    [HttpGet]
    [Route("filterTotalStudents/{byTime?}")]
    public async Task<IActionResult> FilterTotalStudents(string? byTime)
    {
        logger.LogDebug("Index's running! On getting studentDtos...");
        var studentDtos = await dashboardServices.GetLearnersMetrics();
        logger.LogDebug("Got studentDtos!");

        var date = GetByTime(DateTime.Now, byTime);
        var result1 = studentDtos.Where(x => x.CreationTime >= date).ToList();
        var date2 = GetByTime(date, byTime);
        var result2 = studentDtos.Where(x => x.CreationTime >= date2 && x.CreationTime <= date).ToList();


        return Helper.RenderRazorViewToString(this, "_TotalStudents", new TotalValueModel<MetricObject>()
        {
            Models = result1,
            IsIncrease = result1.Count > result2.Count,
            IncreasePercentage = Math.Abs(result1.Count - result2.Count) * 1.0 / result2.Count * 100,
            Time = byTime ?? "Today"
        });
    }

    private static DateTime GetByTime(DateTime date, string? byTime)
    {
        switch (byTime)
        {
            case ByTime.Month:
                date = date.Subtract(TimeSpan.FromDays(29));
                break;

            case ByTime.Week:
                date = date.Subtract(TimeSpan.FromDays(6));
                break;

            case ByTime.Year:
                date = date.Subtract(TimeSpan.FromDays(364));
                break;

            default:
                if (date.Hour == 0)
                {
                    return date.Subtract(TimeSpan.FromDays(1));
                }
                else
                {
                    date = date.Subtract(date.TimeOfDay);
                }

                break;
        }

        return date;
    }

    [Route("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("Error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<(string, string)> GetLineChart(string byTime)
    {
        logger.LogDebug("On getting lineChartData...");
        var lineChartData = await dashboardServices.GetLineChartData(byTime);

        var datesWeekData = new ChartDataType(
            lineChartData.Dates
        );
        var check = JsonConvert.SerializeObject(lineChartData.LineData).Replace("\"Name\"", "name")
            .Replace("\"Data\"", "data");
        var check1 = JsonConvert.SerializeObject(datesWeekData);

        return (check, check1);
    }

    private async Task<PieChartViewModel> GetDonutChart(string byTime)
    {
        logger.LogDebug("filterPieChart's running! On getting DonutChartData...");
        var donutChartData = await dashboardServices.GetDonutChartData(byTime);

        var check2 = JsonConvert.SerializeObject(donutChartData.Names);
        var check1 = JsonConvert.SerializeObject(donutChartData.Values);

        return new PieChartViewModel()
        {
            Labels = check2,
            Series = check1,
            ByTime = byTime
        };
    }

    private async Task<List<string>> AreaChartDataCalculate(string byTime = "")
    {
        logger.LogDebug("filterPieChart's running! On getting DonutChartData...");
        var areaChartData = await dashboardServices.GetAreaChartData(byTime);
        logger.LogDebug("Got donutChartData! Serializing and return");

        var check1 = JsonConvert.SerializeObject(areaChartData.Dates);
        var check2 = JsonConvert.SerializeObject(areaChartData.TotalRevenue.Data);
        var check3 = JsonConvert.SerializeObject(areaChartData.Cancels.Data);
        var check4 = JsonConvert.SerializeObject(areaChartData.Incoming.Data);

        return [check1, check2, check3, check4];
    }
}