using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.ServiceImpls.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public class GetCourseRequestDetailByTutorIdQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IUserRepository userRepository,
    ITutorRepository tutorRepository,
    IIdentityRepository identityRepository,
    ICurrentUserService currentUserService,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetCourseRequestDetailByTutorIdQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestDetailByTutorIdQuery, CourseRequestForDetailDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseRequestForDetailDto>> Handle(GetCourseRequestDetailByTutorIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var courseRequestQueryable =
                from courseFromDb in courseRepository.GetAll()
                join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
                join tutorFromDb1 in tutorRepository.GetAll() on courseFromDb.TutorId equals tutorFromDb1.Id
                join tutorFromDb in userRepository.GetAll() on tutorFromDb1.UserId equals tutorFromDb.Id
                join identityFromDb in identityRepository.GetAll() on tutorFromDb.Id equals identityFromDb.Id
                where courseFromDb.CourseRequests.Any(x =>
                    x.Id == CourseRequestId.Create(request.CourseRequestId)
                    && x.TutorId == TutorId.Create(currentUserService.UserId))
                select new
                {
                    Course = courseFromDb,
                    CourseRequest =
                        courseFromDb.CourseRequests.Where(x => x.Id == CourseRequestId.Create(request.CourseRequestId)),
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