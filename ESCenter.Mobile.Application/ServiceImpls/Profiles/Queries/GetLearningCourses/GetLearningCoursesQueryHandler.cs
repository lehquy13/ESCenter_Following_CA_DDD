using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using TypeAdapter = Mapster.TypeAdapter;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourses;

public class GetLearningCoursesQueryHandler(
    IReadOnlyRepository<Course, CourseId> courseRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    ICurrentUserService currentUserService,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetLearningCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetLearningCoursesQuery, IEnumerable<LearningCourseForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<LearningCourseForListDto>>> Handle(GetLearningCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var coursesQuery = courseRepository.GetAll()
            .Where(x => x.LearnerId == CustomerId.Create(currentUserService.UserId))
            .Join(subjectRepository.GetAll(),
                course => course.SubjectId,
                subject => subject.Id,
                (course, subject) => new { course, subject });
        var courses = await asyncQueryableExecutor
            .ToListAsync(coursesQuery, false, cancellationToken);
        var coursesDtos = courses
            .Select(x => (x.course, x.subject).Adapt<LearningCourseForListDto>())
            .ToList();

        return coursesDtos;
    }
}