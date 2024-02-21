using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetAllCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetAllCoursesQuery, List<CourseForListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<List<CourseForListDto>>> Handle(GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            //Create a list of class query
            var classesQuery = courseRepository.GetAll()
                .OrderByDescending(x => x.CreationTime)
                .Where(x => x.IsDeleted == false)
                .Join(subjectRepository.GetAll(),
                    course => course.SubjectId,
                    subject => subject.Id,
                    (course, subject) => new Tuple<Course, string>(course, subject.Name)
                );

            var courses = await asyncQueryableExecutor.ToListAsync(classesQuery, false, cancellationToken);

            //Get the class of the page
            var classInformationDtos =
                Mapper.Map<List<CourseForListDto>>(courses);

            var class1 = (courses[0].Item1,courses[0].Item2).Adapt<CourseForListDto>();

            return classInformationDtos;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}