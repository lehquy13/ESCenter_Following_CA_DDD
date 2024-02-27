using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetCourseDetail;

public class GetCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IUserRepository userRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
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
            var courseFromDbQ =
                from course in courseRepository.GetAll()
                join sub in subjectRepository.GetAll() on course.SubjectId equals sub.Id
                where course.Id == CourseId.Create(request.CourseId)
                select new Tuple<Course, Subject>(course, sub);

            var courseFromDb =
                await asyncQueryableExecutor.FirstOrDefaultAsync(courseFromDbQ, false, cancellationToken);

            if (courseFromDb is null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

            var classDto = Mapper.Map<CourseForDetailDto>(courseFromDb);

            if (courseFromDb.Item1.TutorId is not null)
            {
                var tutor = await userRepository.GetTutor(courseFromDb.Item1.TutorId);

                if (tutor is null)
                {
                    return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
                }

                classDto.TutorName = tutor.GetFullName();
                classDto.TutorEmail = tutor.Email;
                classDto.TutorPhoneNumber = tutor.PhoneNumber;
            }

            return classDto;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}