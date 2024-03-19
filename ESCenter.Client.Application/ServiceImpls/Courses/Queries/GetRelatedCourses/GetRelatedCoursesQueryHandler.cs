using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetRelatedCourses;

public class GetRelatedCoursesQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IMapper mapper)
    : QueryHandlerBase<GetRelatedCoursesQuery, IEnumerable<CourseForListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<IEnumerable<CourseForListDto>>> Handle(GetRelatedCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var originCourse = await courseRepository.GetAsync(CourseId.Create(request.CourseId), cancellationToken);

        if (originCourse is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var courses = courseRepository
            .GetAll()
            .Where(x => x.SubjectId == originCourse.SubjectId && x.Id != originCourse.Id)
            .Join(subjectRepository.GetAll(),
                course => course.SubjectId,
                subject => subject.Id,
                (course, subject) => new { course, subject.Name }
            );

        var coursesResult = await asyncQueryableExecutor.ToListAsync(courses, false, cancellationToken);

        var courseDtos = coursesResult
            .Select(x => (x.course,x.Name).Adapt<CourseForListDto>())
            .ToList();

        return courseDtos;
    }
}