using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Queries.GetCourses;

public class GetCoursesByIdsQueryHandler(
    ICourseRepository courseRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCoursesByIdsQuery, IEnumerable<CourseForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<CourseForListDto>>> Handle(GetCoursesByIdsQuery request,
        CancellationToken cancellationToken)
    {
        var courseIds = request.Guids.Select(CourseId.Create);

        //Create a list of class query
        var courseQuery =
            from course in courseRepository.GetAll().Where(x => courseIds.Contains(x.Id))
                .OrderByDescending(x => x.CreationTime)
                .Where(x => x.IsDeleted == false && x.Status == Status.Available)
            join subject in subjectRepository.GetAll().Where(x => x.IsDeleted == false) on course.SubjectId equals
                subject.Id
            select new
            {
                Course = course,
                Subject = subject.Name
            };

        var classesQueryResult =
            await asyncQueryableExecutor.ToListAsync(courseQuery, false, cancellationToken);

        //Get the class of the page
        var classInformationDtos = classesQueryResult
            .Select(classR => (classR.Course, classR.Subject).Adapt<CourseForListDto>()).ToList();

        return Result<IEnumerable<CourseForListDto>>.Success(classInformationDtos);
    }
}