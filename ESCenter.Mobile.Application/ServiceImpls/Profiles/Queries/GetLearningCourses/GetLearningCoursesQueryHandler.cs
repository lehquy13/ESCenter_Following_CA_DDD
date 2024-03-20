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
    IAppLogger<GetLearningCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetLearningCoursesQuery, IEnumerable<LearningCourseForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<LearningCourseForListDto>>> Handle(GetLearningCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await courseRepository
            .GetLearningCoursesByUserId(CustomerId.Create(currentUserService.UserId));
        var coursesDtos = Mapper.Map<List<LearningCourseForListDto>>(courses);

        return coursesDtos;
    }
}