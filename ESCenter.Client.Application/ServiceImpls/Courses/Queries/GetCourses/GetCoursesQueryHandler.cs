using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using MapsterMapper;
using Matt.Paginated;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourses;

public class GetCoursesQueryHandler(
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
    ITutorRepository tutorRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCoursesQuery, PaginatedList<CourseForListDto>>(logger, mapper)
{
    public override async Task<Result<PaginatedList<CourseForListDto>>> Handle(GetCoursesQuery request,
        CancellationToken cancellationToken)
    {
        //Create a list of class query
        var courseQuery =
            from course in courseRepository.GetAll().OrderByDescending(x => x.CreationTime)
                .Where(x => x.IsDeleted == false && x.Status == Status.Available)
            join subject in subjectRepository.GetAll().Where(x => x.IsDeleted == false)
                on course.SubjectId equals subject.Id
            select new
            {
                Course = course,
                Subject = subject.Name
            };

        //Filter by SubjectName if it is not null
        if (!string.IsNullOrWhiteSpace(request.CourseParams.SubjectName))
        {
            courseQuery = courseQuery.Where(x =>
                x.Subject.ToLower().Contains(request.CourseParams.SubjectName.ToLower()));
        }

        if (currentUserService.IsAuthenticated)
        {
            if (currentUserService.Roles.Contains("Tutor"))
            {
                // Select the course that his major matches most
                var tutor = await tutorRepository
                    .GetTutorByUserId(CustomerId.Create(currentUserService.UserId),
                        cancellationToken);

                if (tutor != null)
                {
                    courseQuery = courseQuery.Where(x =>
                        x.Course.CourseRequests.All(y => y.TutorId != tutor.Id));

                    // Order by tutor's majors

                    var tutorMajors = tutor.TutorMajors.Select(x => x.SubjectId).ToList();

                    courseQuery = courseQuery.OrderBy(x =>
                        tutorMajors.Contains(x.Course.SubjectId) ? 0 : 1);
                }
            }
        }

        var count = await asyncQueryableExecutor.CountAsync(courseQuery, cancellationToken);

        var classesQueryResult =
            await asyncQueryableExecutor.ToListAsync(
                courseQuery
                    .Skip((request.CourseParams.PageIndex - 1) * request.CourseParams.PageSize)
                    .Take(request.CourseParams.PageSize),
                false, cancellationToken);

        //Get the class of the page
        var classInformationDtos = classesQueryResult
            .Select(classR => (classR.Course, classR.Subject).Adapt<CourseForListDto>()).ToList();

        return PaginatedList<CourseForListDto>.Create(classInformationDtos, request.CourseParams.PageIndex,
            request.CourseParams.PageSize, count);
    }
}