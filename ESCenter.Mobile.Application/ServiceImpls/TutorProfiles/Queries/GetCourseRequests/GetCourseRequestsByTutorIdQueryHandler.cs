using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequests;

public class GetCourseRequestsByTutorIdQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<GetCourseRequestsByTutorIdQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestsByTutorIdQuery, IEnumerable<CourseRequestForListDto>>(unitOfWork, logger,
        mapper)
{
    public override async Task<Result<IEnumerable<CourseRequestForListDto>>> Handle(
        GetCourseRequestsByTutorIdQuery requests,
        CancellationToken cancellationToken)
    {
        var tutorId = IdentityGuid.Create(currentUserService.UserId);
        var courseRequestQueryable =
            from courseFromDb in courseRepository.GetAll()
            join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
            where courseFromDb.CourseRequests.Any(x => x.TutorId == tutorId)
            select new
            {
                Title = courseFromDb.Title,
                CourseRequest =
                    courseFromDb.CourseRequests.Where(x => x.TutorId == tutorId),
                SubjectName = subjectFromDb.Name
            };

        var courseRequests =
            await asyncQueryableExecutor.ToListAsync(courseRequestQueryable, false, cancellationToken);

        var courseRequestDtos = Mapper.Map<List<CourseRequestForListDto>>(courseRequests);

        return courseRequestDtos;
    }
}