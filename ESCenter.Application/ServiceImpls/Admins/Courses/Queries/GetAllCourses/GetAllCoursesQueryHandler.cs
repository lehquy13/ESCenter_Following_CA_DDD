using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetAllCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetAllCoursesQuery, IEnumerable<CourseForListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<IEnumerable<CourseForListDto>>> Handle(GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            //Create a list of class query
            var classesQuery = courseRepository.GetAll()
                .OrderByDescending(x => x.CreationTime)
                .Where(x => x.IsDeleted == false);

            var courses = await asyncQueryableExecutor.ToListAsync(classesQuery, false, cancellationToken);

            //Get the class of the page
            var classInformationDtos =
                Mapper.Map<List<CourseForListDto>>(courses);

            return classInformationDtos;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}