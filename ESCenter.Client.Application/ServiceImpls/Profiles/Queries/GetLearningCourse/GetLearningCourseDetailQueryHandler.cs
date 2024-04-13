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
        var courseId = CourseId.Create(request.CourseId);

        var courseRequestQueryable = courseRepository.GetAll()
            .Where(result => result.Id == courseId)
            .Join(subjectRepository.GetAll(),
                courseFromDb => courseFromDb.SubjectId,
                subjectFromDb => subjectFromDb.Id,
                (courseFromDb, subjectFromDb) => new { Course = courseFromDb, Subject = subjectFromDb })
            .Join(tutorRepository.GetAll(),
                courseSubject => courseSubject.Course.TutorId,
                tutor => tutor.Id,
                (courseSubject, tutor) => new { CourseSubject = courseSubject, Tutor = tutor })
            .Join(customerRepository.GetAll(),
                joined => joined.Tutor.CustomerId,
                customer => customer.Id,
                (joined, customer) => new
                {
                    Course = joined.CourseSubject.Course,
                    Subject = joined.CourseSubject.Subject,
                    TutorId = joined.Tutor.Id,
                    TutorInfo = customer
                });

        var course =
            await asyncQueryableExecutor.SingleOrDefault(courseRequestQueryable, false, cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var classDto = (course.Course, course.Subject, course.TutorId.Value, course.TutorInfo)
            .Adapt<LearningCourseDetailForClientDto>();

        return classDto;
    }
}