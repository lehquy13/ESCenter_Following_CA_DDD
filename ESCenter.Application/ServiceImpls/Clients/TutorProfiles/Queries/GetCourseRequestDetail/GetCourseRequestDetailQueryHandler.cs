using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.ServiceImpls.Admins.Courses;
using ESCenter.Domain.Aggregates.CourseRequests;
using ESCenter.Domain.Aggregates.CourseRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetCourseRequestDetail;

public class GetCourseRequestDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IUserRepository userRepository,
    IIdentityRepository identityRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetCourseRequestDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestDetailQuery, CourseRequestForDetailDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseRequestForDetailDto>> Handle(GetCourseRequestDetailQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var courseRequestQueryable =
                from courseFromDb in courseRepository.GetAll()
                join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
                join tutorFromDb in userRepository.GetAll() on courseFromDb.TutorId equals tutorFromDb.Id
                join identityFromDb in identityRepository.GetAll() on tutorFromDb.Id equals identityFromDb.Id
                where courseFromDb.CourseRequests.Any(
                    x => x.Id == CourseRequestId.Create(request.CourseRequestId))
                select new
                {
                    Course = courseFromDb,
                    CourseRequest = courseFromDb.CourseRequests.Where(x => x.Id == CourseRequestId.Create(request.CourseRequestId)),
                    Subject = subjectFromDb.Name,
                    Tutor = tutorFromDb,
                    IdentityUser = identityFromDb
                };

            var courseRequests = await asyncQueryableExecutor
                .SingleOrDefault(courseRequestQueryable, false, cancellationToken);

            if (courseRequests == null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

            var courseRequestDtos = Mapper.Map<CourseRequestForDetailDto>(courseRequests);

            return courseRequestDtos;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.InnerException!.Message);
            return Result.Fail(ex.Message);
        }
    }
}