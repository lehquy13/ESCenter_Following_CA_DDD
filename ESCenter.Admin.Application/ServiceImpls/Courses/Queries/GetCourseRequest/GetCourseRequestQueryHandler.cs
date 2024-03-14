using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseRequest;

public class GetCourseRequestQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<GetCourseRequestQueryHandler> logger,
    IReadOnlyRepository<Course, CourseId> courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestQuery, CourseRequestCancelDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseRequestCancelDto>> Handle(GetCourseRequestQuery request,
        CancellationToken cancellationToken)
    {
        var courseRequest =
            courseRepository.GetAll()
                .SelectMany(x => x.CourseRequests)
                .Where(x => x.Id == CourseRequestId.Create(request.CourseRequestId));

        var result = await asyncQueryableExecutor.FirstOrDefaultAsync(courseRequest, false, cancellationToken);

        if (result is null)
        {
            return Result.Fail(CourseAppServiceErrors.NonExistCourseRequestError);
        }

        return Mapper.Map<CourseRequestCancelDto>(result);
    }
}