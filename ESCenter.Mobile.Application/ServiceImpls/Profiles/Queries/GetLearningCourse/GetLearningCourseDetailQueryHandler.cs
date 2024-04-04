using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Specifications.Tutors;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.ServiceImpls.Courses;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public class GetLearningCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    ICustomerRepository customerRepository,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper
)
    : QueryHandlerBase<GetLearningCourseDetailQuery, LearningCourseDetailForClientDto>(logger, mapper)
{
    public override async Task<Result<LearningCourseDetailForClientDto>> Handle(GetLearningCourseDetailQuery request,
        CancellationToken cancellationToken)
    {
        var courseRequestQueryable =
            from courseFromDb in courseRepository.GetAll()
            join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
            where courseFromDb.Id == CourseId.Create(request.CourseId)
            select new
            {
                Course = courseFromDb,
                Subject = subjectFromDb
            };

        var courseWithSubject =
            await asyncQueryableExecutor.FirstOrDefaultAsync(courseRequestQueryable, false, cancellationToken);

        if (courseWithSubject is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        if (courseWithSubject.Course.TutorId is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseHasNoTutor);
        }

        var tutor = await tutorRepository.GetAsync(courseWithSubject.Course.TutorId, cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(CourseAppServiceErrors.TutorDoesNotExist);
        }

        var tutorInfo = await customerRepository.GetAsync(tutor.CustomerId, cancellationToken);

        if (tutorInfo is null)
        {
            return Result.Fail(CourseAppServiceErrors.TutorDoesNotExist);
        }

        var classDto = (courseWithSubject.Course, courseWithSubject.Subject, tutor.Id.Value, tutorInfo)
            .Adapt<LearningCourseDetailForClientDto>();

        return classDto;
    }
}