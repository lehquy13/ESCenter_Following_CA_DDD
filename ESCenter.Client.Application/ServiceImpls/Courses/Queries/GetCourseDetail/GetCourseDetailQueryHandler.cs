using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourseDetail;

public class GetCourseDetailQueryHandler(
    IReadOnlyRepository<Course, CourseId> courseRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    IReadOnlyRepository<User, IdentityGuid> userRepository,
    IReadOnlyRepository<Tutor, TutorId> tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetCourseDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseDetailQuery, CourseForDetailDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseForDetailDto>> Handle(GetCourseDetailQuery request,
        CancellationToken cancellationToken)
    {
        var courseFromDbQ =
            from course in courseRepository.GetAll()
            join sub in subjectRepository.GetAll() on course.SubjectId equals sub.Id
            where course.Id == CourseId.Create(request.CourseId)
            select new { course, sub };

        var courseFromDb =
            await asyncQueryableExecutor.FirstOrDefaultAsync(courseFromDbQ, false, cancellationToken);

        if (courseFromDb is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var classDto = (courseFromDb.course, courseFromDb.sub).Adapt<CourseForDetailDto>();

        return classDto;
    }
}