using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetCourseDetail;

public class GetCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<GetCourseDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseDetailQuery, CourseForDetailDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseForDetailDto>> Handle(GetCourseDetailQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var courseFromDb = await courseRepository.GetAsync(CourseId.Create(request.CourseId), cancellationToken);

            if (courseFromDb == null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

            var classDto = Mapper.Map<CourseForDetailDto>(courseFromDb);
            return classDto;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}