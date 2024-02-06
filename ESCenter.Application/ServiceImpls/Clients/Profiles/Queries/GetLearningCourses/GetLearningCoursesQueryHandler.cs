using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.ServiceImpls.Accounts;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Clients.Profiles.Queries.GetLearningCourses;

public class GetLearningCoursesQueryHandler(
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<GetLearningCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetLearningCoursesQuery, IEnumerable<LearningCourseForListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<IEnumerable<LearningCourseForListDto>>> Handle(GetLearningCoursesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(currentUserService.CurrentUserId))
            {
                return Result.Fail(AccountServiceError.UnauthorizedError);
            }
            
            var courses = await courseRepository
                .GetLearningCoursesByUserId(IdentityGuid.Create(
                    new Guid(currentUserService.CurrentUserId)));

            var classInformationDtos = Mapper.Map<List<LearningCourseForListDto>>(courses);

            return classInformationDtos;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.InnerException!.Message);
            return Result.Fail(ex.Message);
        }
    }
}