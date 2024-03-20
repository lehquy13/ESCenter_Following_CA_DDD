using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.ServiceImpls.Courses;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public class GetCourseRequestDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    ICustomerRepository customerRepository,
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetCourseRequestDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestDetailQuery, CourseRequestForDetailDto>(logger, mapper)
{
    public override async Task<Result<CourseRequestForDetailDto>> Handle(GetCourseRequestDetailQuery request,
        CancellationToken cancellationToken)
    {
        var courseRequestQueryable =
            from courseFromDb in courseRepository.GetAll()
            join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
            join tutorFromDb1 in tutorRepository.GetAll() on courseFromDb.TutorId equals tutorFromDb1.Id
            join tutorFromDb in customerRepository.GetAll() on tutorFromDb1.UserId equals tutorFromDb.Id
            where courseFromDb.CourseRequests.Any(
                x => x.Id == CourseRequestId.Create(request.CourseRequestId))
            select new
            {
                Course = courseFromDb,
                CourseRequest =
                    courseFromDb.CourseRequests.Where(x => x.Id == CourseRequestId.Create(request.CourseRequestId)),
                Subject = subjectFromDb.Name,
                Customer = tutorFromDb
            };

        var courseRequests = await asyncQueryableExecutor
            .SingleOrDefault(courseRequestQueryable, false, cancellationToken);

        if (courseRequests == null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        // TODO: Check if the mapper is working
        var courseRequestDtos =
            (courseRequests.Course, courseRequests.CourseRequest, courseRequests.Subject, courseRequests.Customer)
            .Adapt<CourseRequestForDetailDto>();

        return courseRequestDtos;
    }
}