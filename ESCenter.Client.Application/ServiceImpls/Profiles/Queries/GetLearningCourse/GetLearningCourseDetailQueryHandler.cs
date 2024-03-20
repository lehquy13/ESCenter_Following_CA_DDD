using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.ServiceImpls.Courses;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public class GetLearningCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    ICustomerRepository customerRepository,
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ICurrentUserService currentUserService,
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
            join tutor in tutorRepository.GetAll() on courseFromDb.TutorId equals tutor.Id
            join customer in customerRepository.GetAll() on tutor.UserId equals customer.Id
            where courseFromDb.TutorId == CustomerId.Create(currentUserService.UserId) &&
                  courseFromDb.Id == CourseId.Create(request.CourseId)
            select new
            {
                Course = courseFromDb,
                Subject = subjectFromDb,
                TutorId = tutor.Id,
                TutorInfo = customer
            };

        var course =
            await asyncQueryableExecutor.SingleOrDefault(courseRequestQueryable, false, cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var classDto = (course.Course,course.Subject,course.TutorId, course.TutorInfo).Adapt<LearningCourseDetailForClientDto>();

        return classDto;
    }
}