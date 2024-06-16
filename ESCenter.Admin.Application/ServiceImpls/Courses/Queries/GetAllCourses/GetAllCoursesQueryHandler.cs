using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetAllCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetAllCoursesQuery, IEnumerable<CourseForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<CourseForListDto>>> Handle(GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        //Create a list of class query
        var filterQuery = courseRepository.GetAll();

        if (request.Status != Status.None)
        {
            filterQuery = filterQuery.Where(x => x.Status == request.Status);
        }

        var subjects = await subjectRepository.GetAllListAsync(cancellationToken);

        var classesQuery = filterQuery
            .OrderByDescending(x => x.CreationTime)
            .Where(x => x.IsDeleted == false);

        var courses = await asyncQueryableExecutor.ToListAsync(classesQuery, false, cancellationToken);

        //Get the class of the page
        var classInformationDtos = courses
            .Select(x => new CourseForListDto
            {
                Id = x.Id.Value,
                Title = x.Title,
                Status = x.Status.ToString(),
                CreationTime = x.CreationTime,
                LearningMode = x.LearningMode.ToString(),
                SubjectId = x.SubjectId.Value,
                SubjectName = subjects.First(s => s.Id == x.SubjectId).Name,
            }).ToList();

        return classInformationDtos;
    }
}