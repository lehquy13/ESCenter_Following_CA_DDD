using ESCenter.Application.Contract.Charts;
using ESCenter.Application.Contract.Interfaces.DashBoards;
using ESCenter.Application.Contract.Notifications;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Application.ServiceImpls;

internal class DashboardServices(
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUserRepository userRepository,
    IIdentityRepository identityRepository,
    IRepository<Notification, int> notificationRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IAppLogger<DashboardServices> logger)
    : BaseAppService<DashboardServices>(mapper, unitOfWork, logger), IDashboardServices
{
    //TODO: Add IUserRepository


    public async Task<Result<AreaChartData>> GetAreaChartData(GetAreaChartDataQuery getAreaChartDataQuery)
    {
        await Task.CompletedTask;

        // Create a dateListRange by the query.ByTime
        List<int> dates = new List<int>();

        var startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));
        switch (getAreaChartDataQuery.ByTime)
        {
            case ByTime.Month:
                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(29));
                for (int i = 0; i < 30; i++)
                {
                    dates.Add(startDay.Day);
                    startDay = startDay.AddDays(1);
                }

                break;
            case ByTime.Week:
                for (int i = 0; i < 7; i++)
                {
                    dates.Add(startDay.Day);
                    startDay = startDay.AddDays(1);
                }

                break;
        }

        startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));

        var allClasses = await courseRepository.GetListAsync();
        var confirmedClasses = dates.GroupJoin(
            allClasses
                .Where(x => x.CreationTime >= startDay && x.Status == Status.Confirmed)
                .GroupBy(x => x.CreationTime.Day), // Group by day of time range, then merge with dates
            d => d,
            c => c.Key,
            (d, c) => new
            {
                dates = d,
                sum = c.FirstOrDefault()?.Sum(r => r.ChargeFee.Amount) ?? 0
            });
        var canceledClasses = dates.GroupJoin(
            allClasses
                .Where(x => x.CreationTime >= startDay && x.Status == Status.Canceled)
                .GroupBy(x => x.CreationTime.Day),
            d => d,
            c => c.Key,
            (d, c) => new
            {
                dates = d,
                sum = c.FirstOrDefault()?.Sum(r => r.ChargeFee.Amount) ?? 0
            });
        var onPurchasingClasses = dates.GroupJoin(
            allClasses
                .Where(x => x.CreationTime >= startDay && x.Status == Status.OnPurchasing)
                .GroupBy(x => x.CreationTime.Day),
            d => d,
            c => c.Key,
            (d, c) => new
            {
                dates = d,
                sum = c.FirstOrDefault()?.Sum(r => r.ChargeFee.Amount) ?? 0
            });


        List<float> resultInts = confirmedClasses
            .Select(x => x.sum)
            .ToList();
        if (resultInts.Count <= 0)
        {
            resultInts.Add(1);
        }

        List<float> resultInts1 = canceledClasses
            .Select(x => x.sum)
            .ToList();
        if (resultInts1.Count <= 0)
        {
            resultInts1.Add(1);
        }

        List<float> resultInts2 = onPurchasingClasses
            .Select(x => x.sum)
            .ToList();
        if (resultInts2.Count <= 0)
        {
            resultInts2.Add(1);
        }

        var resultStrings = dates
            .Select(x => x.ToString()).ToList();

        if (resultStrings.Count <= 0)
        {
            resultStrings.Add("None");
        }


        return new AreaChartData
        (
            new AreaData("Total Revenues", resultInts),
            new AreaData("Charged", resultInts2),
            new AreaData("Refunded", resultInts1),
            resultStrings
        );
    }

    public async Task<Result<DonutChartData>> GetDonutChartData(GetDonutChartDataQuery getDonutChartDataQuery)
    {
        await Task.CompletedTask;

        var startDay = DateTime.Today;
        switch (getDonutChartDataQuery.ByTime)
        {
            case ByTime.Month:
                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(29));

                break;
            case ByTime.Week:
                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));
                break;
        }

        var classInforsPieQuery = courseRepository.GetAll()
            .Where(x => x.IsDeleted == false && x.LastModificationTime >= startDay)
            .GroupBy(x => x.Status)
            .Select((x) => new { key = x.Key.ToString(), count = x.Count() });

        var classInforsPie = await asyncQueryableExecutor.ToListAsync(classInforsPieQuery, false);

        List<int> resultInts = classInforsPie
            .Select(x => x.count)
            .ToList();

        if (resultInts.Count <= 0)
        {
            resultInts.Add(1);
        }

        List<string> resultStrings = classInforsPie
            .Select(x => x.key)
            .ToList();
        if (resultStrings.Count <= 0)
        {
            resultStrings.Add("None");
        }

        return new DonutChartData(resultInts, resultStrings);
    }

    public async Task<Result<LineChartData>> GetLineChartData(GetLineChartDataQuery getLineChartDataQuery)
    {
        await Task.CompletedTask;
        List<int> dates = new List<int>();

        var startDay = DateTime.Today;
        switch (getLineChartDataQuery.ByTime)
        {
            case "month":
                startDay = startDay.Subtract(TimeSpan.FromDays(29));

                for (int i = 0; i < 30; i++)
                {
                    dates.Add(startDay.Day);
                    startDay = startDay.AddDays(1);
                }

                startDay = startDay.Subtract(TimeSpan.FromDays(29));

                break;
            default:
                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));

                for (int i = 0; i < 7; i++)
                {
                    dates.Add(startDay.Day);
                    startDay = startDay.AddDays(1);
                }

                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));

                break;
        }

        var allClasses = courseRepository.GetAll()
            .Where(x => x.CreationTime >= startDay)
            .GroupBy(x => x.CreationTime.Day).ToList();

        var allUserToDay =
            from iden in identityRepository.GetAll()
            join user in userRepository.GetAll() on iden.Id equals user.Id
            where user.CreationTime >= startDay && iden.IdentityRoleId.Value == IdentityRole.Learner
            group user by user.CreationTime.Day
            into g
            select new
            {
                IdentityLearner = g
            };

        var allTutorToday =
            from iden in identityRepository.GetAll()
            join user in userRepository.GetAll() on iden.Id equals user.Id
            where user.CreationTime >= startDay && iden.IdentityRoleId.Value == IdentityRole.Tutor
            group user by user.CreationTime.Day
            into g
            select new
            {
                IdentityLearner = g
            };

        var allLearner = await asyncQueryableExecutor.ToListAsync(allUserToDay, false);
        var allTutor = await asyncQueryableExecutor.ToListAsync(allTutorToday, false);


        var classesInWeek =
        (
            from d in dates
            join c in allClasses on d equals c.Key
                into dateClassesGroup
            from cl in dateClassesGroup.DefaultIfEmpty()
            select new
            {
                classInfo = cl?.Count() ?? 0
            }.classInfo
        ).ToList();

        var studentsInWeek = dates.GroupJoin(
                allLearner,
                d => d,
                c => c.IdentityLearner.Key,
                (d, c) => new
                {
                    dates = d,
                    classInfo = c.FirstOrDefault()?.IdentityLearner.Count() ?? 0
                })
            .Select(x => x.classInfo)
            .ToList();

        var tutorsInWeek = dates.Join(
                allTutor,
                d => d,
                c => c.IdentityLearner.Key,
                (d, c) => new
                {
                    dates = d,
                    classInfo = c.IdentityLearner?.Count() ?? 0
                })
            .Select(x => x.classInfo)
            .ToList();

        var chartWeekData = new List<LineData>()
        {
            new("Classes", classesInWeek),
            new("Tutors", tutorsInWeek),
            new("Students", studentsInWeek)
        };

        return new LineChartData(chartWeekData, dates);
    }

    public async Task<Result<List<NotificationDto>>> GetNotification(GetNotificationQuery getNotificationQuery)
    {
        await Task.CompletedTask;

        // Create a dateListRange
        var notiListQueryable = notificationRepository.GetAll()
            .Where(x => x.CreationTime >= DateTime.Today);
        
        var notiList = await asyncQueryableExecutor.ToListAsync(notiListQueryable, false);
        
        var notiDtoList = Mapper.Map<List<NotificationDto>>(notiList);
        return notiDtoList;
    }
}