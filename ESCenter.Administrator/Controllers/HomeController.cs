using System.Diagnostics;
using ESCenter.Administrator.Models;
using ESCenter.Administrator.Utilities;
using ESCenter.Application.Contracts.Charts;
using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.Contracts.Users.BasicUsers;
using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.Interfaces.DashBoards;
using ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetAllCourses;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutors;
using ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearners;
using ESCenter.Application.ServiceImpls.Clients.Courses.Queries.GetCourses;
using ESCenter.Domain.Shared.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
[Route("[controller]")]
public class HomeController(
    ILogger<HomeController> logger,
    IDashboardServices dashboardServices,
    ISender sender)
    : Controller
{
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var classDtos = await sender.Send(new GetAllCoursesQuery());
        var tutorDtos = await sender.Send(new GetAllTutorsQuery());
        var learner = await sender.Send(new GetLearnersQuery());

        if (classDtos.IsFailure || tutorDtos.IsFailure || learner.IsFailure || classDtos.Value == null ||
            tutorDtos.Value == null || learner.Value == null)
        {
            logger.LogError("Error on getting classDtos, tutorDtos, studentDtos!");
            return RedirectToAction("Error");
        }

        var date = GetByTime(DateTime.Now, ByTime.Month);
        var courseCountByTime = classDtos.Value.Count(x => x.CreationTime >= date);
        var tutorCountByTime = tutorDtos.Value.Count(x => x.CreationTime >= date);
        var learnerCountByTime = learner.Value.Count(x => x.CreationTime >= date);

        date = GetByTime(date, ByTime.Month);
        var courseCountByLastTime = classDtos.Value.Count(x => x.CreationTime >= date);
        var tutorCountByLastTime = tutorDtos.Value.Count(x => x.CreationTime >= date);
        var learnerCountByLastTime = learner.Value.Count(x => x.CreationTime >= date);

        logger.LogDebug("On getting lineChartData, donutChartData...");
        var lineChartData = await dashboardServices.GetLineChartData(new GetLineChartDataQuery());
        var donutChartData = await dashboardServices.GetDonutChartData(new GetDonutChartDataQuery());

        if (lineChartData.IsFailure || donutChartData.IsFailure || lineChartData.Value == null ||
            donutChartData.Value == null)
        {
            logger.LogError("Error on getting lineChartData, donutChartData!");
            return RedirectToAction("Error");
        }

        var datesWeekData = new ChartDataType(
            "string",
            lineChartData.Value.Dates
        );

        logger.LogDebug("Got lineChartData, donutChartData!");

        var chartWeekData = JsonConvert.SerializeObject(lineChartData.Value.LineData);
        var pieWeekDataNames = JsonConvert.SerializeObject(donutChartData.Value.Names);
        var pieWeekDataValues = JsonConvert.SerializeObject(donutChartData.Value.Values);
        var dateWeekData = JsonConvert.SerializeObject(datesWeekData);

        var areaListData = await AreaChartDataCalculate(ByTime.Week);

        logger.LogDebug("On getting recent activities...");
        var notificationDtos = await dashboardServices.GetNotification(new GetNotificationQuery());

        if (notificationDtos.IsFailure || notificationDtos.Value == null)
        {
            logger.LogError("Error on getting recent activities!");
            return RedirectToAction("Error");
        }

        logger.LogInformation("Got recent activities! Serializing and return");
        return View(
            new DashBoardViewModel
            {
                StudentTotalValueModel = new TotalValueModel<UserForListDto>()
                {
                    Models = learner.Value,
                    IsIncrease = learnerCountByTime > learnerCountByLastTime,
                    IncreasePercentage = Math.Abs(learnerCountByTime - learnerCountByLastTime) * 1.0 /
                        learnerCountByLastTime * 100,
                    Time = ByTime.Month
                },
                ClassTotalValueModel = new TotalValueModel<CourseForListDto>()
                {
                    Models = classDtos.Value.ToList(),
                    IsIncrease = courseCountByTime > courseCountByLastTime,
                    IncreasePercentage =
                        Math.Abs(courseCountByTime - courseCountByLastTime) * 1.0 /
                        courseCountByLastTime * 100,
                    Time = ByTime.Month
                },
                TutorTotalValueModel = new TotalValueModel<UserForListDto>()
                {
                    Models = tutorDtos.Value.ToList(),
                    IsIncrease = tutorCountByTime > tutorCountByLastTime,
                    IncreasePercentage = Math.Abs(tutorCountByTime - tutorCountByLastTime) * 1.0 /
                        tutorCountByLastTime * 100,
                    Time = ByTime.Month
                },

                ChartWeekData = chartWeekData,
                // Chart
                PieWeekData1 = pieWeekDataValues,
                PieWeekData2 = pieWeekDataNames,
                DatesWeekData = dateWeekData,
                //Incomes Chart
                AreaChartViewModel = new AreaChartViewModel()
                {
                    Dates = areaListData.ElementAt(0),
                    TotalRevenueSeries = areaListData.ElementAt(1),
                    RefundedSeries = areaListData.ElementAt(2),
                    IncomingSeries = areaListData.ElementAt(3),
                    ByTime = ByTime.Week
                },
                NotificationDtos = notificationDtos.Value
            }
        );
    }

    [HttpGet]
    [Route("filter-line-chart/{byTime?}")]
    public async Task<IActionResult> FilterLineChart(string? byTime)
    {
        logger.LogDebug("On getting lineChartData...");
        var lineChartData = await dashboardServices.GetLineChartData(new GetLineChartDataQuery
        {
            ByTime = byTime ?? ""
        });

        if (lineChartData.IsFailure || lineChartData.Value == null)
        {
            logger.LogError("Error on getting lineChartData!");
            return RedirectToAction("Error");
        }

        var datesWeekData = new ChartDataType(
            "string",
            lineChartData.Value.Dates
        );
        var check = JsonConvert.SerializeObject(lineChartData.Value.LineData);
        var check1 = JsonConvert.SerializeObject(datesWeekData);

        return Json(new
        {
            ChartWeekData = check,
            DatesWeekData = check1
        });
    }

    [HttpGet]
    [Route("filter-pie-chart/{byTime?}")]
    public async Task<IActionResult> FilterPieChart(string? byTime)
    {
        logger.LogDebug("filterPieChart's running! On getting DonutChartData...");
        var donutChartData = await dashboardServices.GetDonutChartData(new GetDonutChartDataQuery
        {
            ByTime = byTime ?? ""
        });

        if (donutChartData.IsFailure || donutChartData.Value == null)
        {
            logger.LogError("Error on getting donutChartData!");
            return RedirectToAction("Error");
        }

        var check2 = JsonConvert.SerializeObject(donutChartData.Value.Names);
        var check1 = JsonConvert.SerializeObject(donutChartData.Value.Values);

        return Helper.RenderRazorViewToString(this, "_PieChart",
            new PieChartViewModel()
            {
                Labels = check2,
                Series = check1,
                ByTime = byTime ?? ByTime.Today
            });
    }

    private async Task<List<string>> AreaChartDataCalculate(string? byTime)
    {
        logger.LogDebug("filterPieChart's running! On getting DonutChartData...");
        var areaChartData = await dashboardServices.GetAreaChartData(new GetAreaChartDataQuery
        {
            ByTime = byTime ?? ""
        });
        logger.LogDebug("Got donutChartData! Serializing and return");

        if (areaChartData.IsFailure || areaChartData.Value == null)
        {
            logger.LogError("Error on getting areaChartData!");
            return new List<string>();
        }

        var check1 = JsonConvert.SerializeObject(areaChartData.Value.Dates);
        var check2 = JsonConvert.SerializeObject(areaChartData.Value.TotalRevenue.Data);
        var check3 = JsonConvert.SerializeObject(areaChartData.Value.Cancels.Data);
        var check4 = JsonConvert.SerializeObject(areaChartData.Value.Incoming.Data);

        return [check1, check2, check3, check4];
    }

    [HttpGet]
    [Route("filter-area-chart/{byTime?}")]
    public async Task<IActionResult> FilterAreaChart(string? byTime)
    {
        var listData = await AreaChartDataCalculate(byTime);
        return Helper.RenderRazorViewToString(this, "_AreaChart",
            new AreaChartViewModel()
            {
                Dates = listData.ElementAt(1),
                TotalRevenueSeries = listData.ElementAt(1),
                RefundedSeries = listData.ElementAt(3),
                IncomingSeries = listData.ElementAt(4),
                ByTime = byTime ?? ByTime.Today
            });
    }

    [HttpGet]
    [Route("filter-total-classes/{byTime?}")]
    public async Task<IActionResult> FilterTotalClasses(string? byTime)
    {
        logger.LogDebug("Index's running! On getting classDtos...");
        var classDtos = await sender.Send(new GetAllCoursesQuery());

        if (classDtos.IsFailure || classDtos.Value == null)
        {
            logger.LogError("Error on getting classDtos!");
            return RedirectToAction("Error");
        }

        var date = GetByTime(DateTime.Now, byTime);
        var result1 = classDtos.Value.Where(x => x.CreationTime >= date).ToList();
        var date2 = GetByTime(date, byTime);
        var result2 = classDtos.Value.Where(x => x.CreationTime >= date2 && x.CreationTime <= date).ToList();

        return Helper.RenderRazorViewToString(this, "_TotalClasses", new TotalValueModel<CourseForListDto>()
        {
            Models = result1,
            IsIncrease = result1.Count > result2.Count,
            IncreasePercentage = (Math.Abs(result1.Count - result2.Count) * 1.0 / result2.Count) * 100,
            Time = byTime ?? "Today"
        });
    }

    [HttpGet]
    [Route("FilterTotalTutors/{byTime?}")]
    public async Task<IActionResult> FilterTotalTutors(string? byTime)
    {
        logger.LogDebug("Index's running! On getting tutorDtos...");
        var tutorDtos = await sender.Send(new GetAllTutorsQuery()); 
                                                             
        if (tutorDtos.IsFailure || tutorDtos.Value == null)          
        {                                                            
            logger.LogError("Error on getting classDtos!");          
            return RedirectToAction("Error");                        
        }                                                            
        
        logger.LogDebug("Got tutorDtos!");
        var date = GetByTime(DateTime.Now, byTime);
        var result1 = tutorDtos.Value.Where(x => x.CreationTime >= date).ToList();
        var date2 = GetByTime(date, byTime);

        var result2 = tutorDtos.Value.Where(x => x.CreationTime >= date2 && x.CreationTime <= date).ToList();

        return Helper.RenderRazorViewToString(this, "_TotalTutors", new TotalValueModel<UserForListDto>()
        {
            Models = result1,
            IsIncrease = result1.Count > result2.Count,
            IncreasePercentage = Math.Abs(result1.Count - result2.Count) * 1.0 / result2.Count * 100,
            Time = byTime ?? "Today"
        });
    }

    [HttpGet]
    [Route("filterTotalStudents/{byTime?}")]
    public async Task<IActionResult> filterTotalStudents(string? byTime)
    {
        logger.LogDebug("Index's running! On getting studentDtos...");
        var studentDtos = await sender.Send(new GetLearnersQuery());
        logger.LogDebug("Got studentDtos!");
        
        if (studentDtos.IsFailure || studentDtos.Value == null)
        {
            logger.LogError("Error on getting studentDtos!");
            return RedirectToAction("Error");
        }

        var date = GetByTime(DateTime.Now, byTime);
        var result1 = studentDtos.Value.Where(x => x.CreationTime >= date).ToList();
        var date2 = GetByTime(date, byTime);
        var result2 = studentDtos.Value.Where(x => x.CreationTime >= date2 && x.CreationTime <= date).ToList();


        return Helper.RenderRazorViewToString(this, "_TotalStudents", new TotalValueModel<UserForListDto>()
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
}