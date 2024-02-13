using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.ServiceImpls.Admins.Courses;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetLearningCourse;

public class GetLearningCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IUserRepository userRepository,
    ITutorRepository tutorRepository,
    IIdentityRepository identityRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper
)
    : QueryHandlerBase<GetLearningCourseDetailQuery, CourseForDetailDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseForDetailDto>> Handle(GetLearningCourseDetailQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var courseRequestQueryable =
                from courseFromDb in courseRepository.GetAll()
                join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
                join tutor1 in tutorRepository.GetAll() on courseFromDb.TutorId equals tutor1.Id
                join tutor in userRepository.GetAll() on tutor1.UserId equals tutor.Id
                join identityFromDb in identityRepository.GetAll() on tutor.Id equals identityFromDb.Id
                where courseFromDb.TutorId == IdentityGuid.Create(request.LearnerId) &&
                      courseFromDb.Id == CourseId.Create(request.CourseId)
                select new
                {
                    Course = courseFromDb,
                    Subject = subjectFromDb,
                    TutorInfo = tutor,
                    Identity = identityFromDb
                };

            var course =
                await asyncQueryableExecutor.SingleOrDefault(courseRequestQueryable, false, cancellationToken);

            if (course is null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

            var classDto = Mapper.Map<CourseForDetailDto>(course);

            return classDto;
        }
        catch (Exception ex)
        {
            Logger.LogError("{Message} {Detail}", ex.Message, ex.InnerException?.Message ?? string.Empty);
            return Result.Fail(ex.Message);
        }
    }
}