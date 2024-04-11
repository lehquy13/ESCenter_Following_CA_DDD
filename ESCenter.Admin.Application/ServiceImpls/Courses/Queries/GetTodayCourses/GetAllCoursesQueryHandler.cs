using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetTodayCourses;

public class GetTodayCoursesQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetTodayCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTodayCoursesQuery, IEnumerable<CourseForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<CourseForListDto>>> Handle(GetTodayCoursesQuery request,
        CancellationToken cancellationToken)
    {
        //Create a list of class query
        var filterQuery = courseRepository.GetAll();

        var classesQuery = filterQuery
            .OrderByDescending(x => x.CreationTime)
            .Where(x => x.IsDeleted == false && x.CreationTime >= DateTime.Today.AddDays(-1))
            .Join(subjectRepository.GetAll(),
                course => course.SubjectId,
                subject => subject.Id,
                (course, subject) => new Tuple<Course, string>(course, subject.Name)
            );

        var courses = await asyncQueryableExecutor.ToListAsync(classesQuery, false, cancellationToken);

        //Get the class of the page
        var classInformationDtos = courses
            .Select(x => (x.Item1, x.Item2).Adapt<CourseForListDto>())
            .ToList();

        return classInformationDtos;
    }
}