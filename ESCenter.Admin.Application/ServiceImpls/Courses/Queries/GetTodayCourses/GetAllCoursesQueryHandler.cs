using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
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
            .Where(x => x.IsDeleted == false && x.CreationTime >= DateTime.Today.AddDays(-1));
            
        var subjects = await subjectRepository.GetAllListAsync(cancellationToken);

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