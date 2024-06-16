using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.ServiceImpls.Courses;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public class GetLearningCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    ICustomerRepository customerRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
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
                (courseFromDb, subjectFromDb) => new { Course = courseFromDb, Subject = subjectFromDb });

        var course =
            await asyncQueryableExecutor.FirstOrDefaultAsync(courseRequestQueryable, false, cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var classDto = new LearningCourseDetailForClientDto
        {
            Id = course.Course.Id.Value,
            Title = course.Course.Title,
            Description = course.Course.Description,
            SubjectName = course.Subject.Name,
            Status = course.Course.Status.ToString(),
            LearningMode = course.Course.LearningMode.ToString(),
            ChargeFee = course.Course.ChargeFee.Amount,
            SectionFee = course.Course.SectionFee.Amount,
            SessionDuration = course.Course.SessionDuration.Value,
            SessionPerWeek = course.Course.SessionPerWeek.Value,
            Address = course.Course.Address,
            Rate = course.Course.Review == null ? (short) 5 : course.Course.Review.Rate,
            Detail = course.Course.Review == null ? "" : course.Course.Review.Detail
        };

        if (course.Course.TutorId is null || course.Course.Status != Status.Confirmed) return classDto;
        
        var tutor = await customerRepository.GetTutorByTutorId(course.Course.TutorId, cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
        }

        classDto.TutorId = tutor.Id.Value;
        classDto.TutorName = tutor.GetFullName();
        classDto.TutorContact = tutor.PhoneNumber;
        classDto.TutorEmail = tutor.Email;

        return classDto;
    }
}