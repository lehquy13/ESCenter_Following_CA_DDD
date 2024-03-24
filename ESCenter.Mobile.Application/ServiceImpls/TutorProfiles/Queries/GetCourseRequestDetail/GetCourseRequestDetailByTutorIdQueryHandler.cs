using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.ServiceImpls.Courses;
using Mapster;
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
    ICustomerRepository customerRepository,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetCourseRequestDetailByTutorIdQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestDetailByTutorIdQuery, CourseRequestForDetailDto>(logger, mapper)
{
    public override async Task<Result<CourseRequestForDetailDto>> Handle(GetCourseRequestDetailByTutorIdQuery request,
        CancellationToken cancellationToken)
    {
        var courseRequestQueryable =
            from courseFromDb in courseRepository.GetAll()
            join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
            join tutor in tutorRepository.GetAll() on courseFromDb.TutorId equals tutor.Id
            join customer in customerRepository.GetAll() on tutor.CustomerId equals customer.Id
            where courseFromDb.CourseRequests.Any(x =>
                x.Id == CourseRequestId.Create(request.CourseRequestId)
                && x.TutorId == TutorId.Create(currentUserService.UserId))
            select new
            {
                Course = courseFromDb,
                CourseRequest =
                    courseFromDb.CourseRequests.Where(x => x.Id == CourseRequestId.Create(request.CourseRequestId)),
                Subject = subjectFromDb.Name,
                Customer = customer
            };

        var courseRequests = await asyncQueryableExecutor
            .SingleOrDefault(courseRequestQueryable, false, cancellationToken);

        if (courseRequests == null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var courseRequestDtos =
            (courseRequests.Course, courseRequests.CourseRequest, courseRequests.Subject, courseRequests.Customer)
            .Adapt<CourseRequestForDetailDto>();

        return courseRequestDtos;
    }
}