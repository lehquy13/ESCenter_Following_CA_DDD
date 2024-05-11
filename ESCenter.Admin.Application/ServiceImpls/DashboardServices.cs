using ESCenter.Admin.Application.Contracts.Charts;
using ESCenter.Admin.Application.Contracts.Notifications;
using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.DashBoards;
using ESCenter.Admin.Application.ServiceImpls.Tutors;
using ESCenter.Application;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls;

internal class DashboardServices(
    IReadOnlyRepository<Course, CourseId> courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ICustomerRepository customerRepository,
    IReadOnlyRepository<Notification, int> notificationRepository,
    IReadOnlyRepository<TutorRequest, TutorRequestId> tutorRequestRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IAppLogger<DashboardServices> logger)
    : BaseAppService<DashboardServices>(mapper, unitOfWork, logger), IDashboardServices
{
    public async Task<AreaChartData> GetAreaChartData(string byTime = "")
    {
        await Task.CompletedTask;

        // Create a dateListRange by the query.ByTime
        List<int> dates = [];

        var startDay = DateTime.Today.Subtract(TimeSpan.FromDays(30));
        switch (byTime)
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

        startDay = DateTime.Today.Subtract(TimeSpan.FromDays(30));

        var allClassesQuery =
            courseRepository
                .GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.CreationTime,
                    x.Status,
                    x.ChargeFee
                });

        var allClasses = await asyncQueryableExecutor.ToListAsync(allClassesQuery, false);

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
                .Where(x => x.CreationTime >= startDay && x.Status == Status.OnProgressing)
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

    public async Task<DonutChartData> GetDonutChartData(string byTime = "")
    {
        await Task.CompletedTask;

        var startDay = byTime switch
        {
            ByTime.Month => DateTime.Today.Subtract(TimeSpan.FromDays(29)),
            ByTime.Week => DateTime.Today.Subtract(TimeSpan.FromDays(6)),
            _ => DateTime.Today
        };

        var courseDonutQuery = courseRepository.GetAll()
            .Where(x => x.IsDeleted == false && x.LastModificationTime >= startDay)
            .GroupBy(x => x.Status)
            .Select((x) => new { key = x.Key.ToString(), count = x.Count() });

        var courseDonut = await asyncQueryableExecutor.ToListAsync(courseDonutQuery, false);

        List<int> resultLabels = courseDonut
            .Select(x => x.count)
            .ToList();

        if (resultLabels.Count <= 0)
        {
            resultLabels.Add(1);
        }

        List<string> resultStrings = courseDonut
            .Select(x => x.key)
            .ToList();
        if (resultStrings.Count <= 0)
        {
            resultStrings.Add("None");
        }

        return new DonutChartData(resultLabels, resultStrings);
    }

    public async Task<LineChartData> GetLineChartData(string byTime = "")
    {
        await Task.CompletedTask;
        List<int> dates = new List<int>();

        var startDay = DateTime.Today;
        switch (byTime)
        {
            case ByTime.Month:
                startDay = startDay.Subtract(TimeSpan.FromDays(29));

                for (var i = 0; i < 30; i++)
                {
                    dates.Add(startDay.Day);
                    startDay = startDay.AddDays(1);
                }

                startDay = startDay.Subtract(TimeSpan.FromDays(29));
                break;

            default:
                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));

                for (var i = 0; i < 7; i++)
                {
                    dates.Add(startDay.Day);
                    startDay = startDay.AddDays(1);
                }

                startDay = DateTime.Today.Subtract(TimeSpan.FromDays(6));
                break;
        }


        // Get all the classes in the week, then group by day, select the count of classes in that day
        var allClassesQueryable = courseRepository
            .GetAll()
            .Where(x => x.CreationTime >= startDay)
            .GroupBy(x => x.CreationTime.Day)
            .Select(x => new
            {
                date = x.Key,
                count = x.Count()
            });

        var allClasses = await asyncQueryableExecutor.ToListAsync(allClassesQueryable, false);

        var classesInWeek = dates
            .GroupJoin( // Group by day of time range, then merge with dates
                allClasses,
                d => d,
                c => c.date,
                (d, c) => c.FirstOrDefault()?.count ?? 0)
            .ToList();

        // --------- Learner metrics -----------------
        var allUserToDay =
            customerRepository.GetAll()
                .Where(x => x.CreationTime >= startDay && x.Role == Role.Learner)
                .GroupBy(x => x.CreationTime.Day)
                .Select(x => new
                {
                    date = x.Key,
                    count = x.Count()
                });

        var allLearner = await asyncQueryableExecutor
            .ToListAsync(allUserToDay, false);

        var studentsInWeek = dates
            .GroupJoin(
                allLearner,
                d => d,
                c => c.date,
                (d, c) => c.FirstOrDefault()?.count ?? 0)
            .ToList();

        // --------- Tutor metrics -----------------
        var allTutorToday =
            customerRepository.GetAll()
                .Where(x => x.CreationTime >= startDay && x.Role == Role.Tutor)
                .GroupBy(x => x.CreationTime.Day)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                });
        var allTutor = await asyncQueryableExecutor
            .ToListAsync(allTutorToday, false);

        var tutorsInWeek = dates.GroupJoin(
                allTutor,
                d => d,
                c => c.Key,
                (d, c) => c.FirstOrDefault()?.count ?? 0)
            .ToList();

        // --------- Create the chart data -----------------
        var chartWeekData = new List<LineData>()
        {
            new("Classes", classesInWeek),
            new("Tutors", tutorsInWeek),
            new("Students", studentsInWeek)
        };

        return new LineChartData(chartWeekData, dates);
    }

    public async Task<List<NotificationDto>> GetNotification()
    {
        // Create a dateListRange
        var notiListQueryable = notificationRepository
            .GetAll()
            .OrderBy(x => x.CreationTime)
            .Take(5);
        var notiList = await asyncQueryableExecutor.ToListAsync(notiListQueryable, false);
        var notiDtoList = Mapper.Map<List<NotificationDto>>(notiList);

        return notiDtoList;
    }

    public Task<List<MetricObject>> GetTutorsMetrics()
    {
        var tutors = customerRepository.GetAll()
            .Where(x => x.Role == Role.Tutor)
            .Select(x => new MetricObject
            {
                Id = x.Id,
                CreationTime = x.CreationTime
            });

        return asyncQueryableExecutor.ToListAsync(tutors, false);
    }

    public Task<List<MetricObject>> GetCoursesMetrics()
    {
        var courses = courseRepository.GetAll()
            .Select(x => new MetricObject
            {
                Id = x.Id,
                CreationTime = x.CreationTime
            });

        return asyncQueryableExecutor.ToListAsync(courses, false);
    }

    public Task<List<MetricObject>> GetLearnersMetrics()
    {
        var learners = customerRepository.GetAll()
            .Where(x => x.Role == Role.Learner)
            .Select(x => new MetricObject
            {
                Id = x.Id,
                CreationTime = x.CreationTime
            });

        return asyncQueryableExecutor.ToListAsync(learners, false);
    }

    public async Task<IEnumerable<TutorRequestForListDto>> GetLatestTutorRequests()
    {
        var queryable =
            tutorRequestRepository.GetAll()
                .OrderByDescending(x => x.RequestStatus)
                .ThenByDescending(x => x.CreationTime)
                .Take(10)
                .Join(customerRepository.GetAll(),
                    req => req.LearnerId,
                    user => user.Id,
                    (req, user) => new TutorRequestForListDto
                    {
                        Id = req.Id.Value,
                        TutorId = req.TutorId.Value,
                        LearnerId = req.LearnerId.Value,
                        PhoneNumber = user.PhoneNumber,
                        Name = user.FirstName + " " + user.LastName,
                        RequestMessage = req.Message,
                        Status = req.RequestStatus
                    });

        var results = await asyncQueryableExecutor.ToListAsync(
            queryable
            , false);

        foreach (var tutorRequestForListDto in results)
        {
            var tutor = await customerRepository.GetTutorByTutorId(TutorId.Create(tutorRequestForListDto.TutorId));

            if (tutor == null)
            {
                throw new NotFoundException("NonExistTutorOfCreatedRequestError");
            }

            tutorRequestForListDto.TutorEmail = tutor.Email;
            tutorRequestForListDto.TutorFullName = tutor.FirstName + " " + tutor.LastName;
            tutorRequestForListDto.TutorPhoneNumber = tutor.PhoneNumber;
        }

        return results;
    }
}