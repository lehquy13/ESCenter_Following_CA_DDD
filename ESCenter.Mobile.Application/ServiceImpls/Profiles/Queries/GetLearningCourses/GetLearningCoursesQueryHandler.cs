using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourses;

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
            var courses = await courseRepository
                .GetLearningCoursesByUserId(IdentityGuid.Create(currentUserService.UserId));

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