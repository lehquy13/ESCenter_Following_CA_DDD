using ESCenter.Admin.Application.Contracts.Courses.Dtos;
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

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseDetail;

public class GetCourseDetailQueryHandler(
    IReadOnlyRepository<Course, CourseId> courseRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    IReadOnlyRepository<Customer, CustomerId> userRepository,
    IReadOnlyRepository<Tutor, TutorId> tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetCourseDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseDetailQuery, CourseForDetailDto>(logger, mapper)
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
        var a = courseFromDb.course.CourseRequests.Select(x => x.TutorId);

        var courseRequestsQ =
            tutorRepository.GetAll()
                .Where(x => a.Any(cr => cr == x.Id))
                .Join(userRepository.GetAll(),
                    tutor => tutor.UserId,
                    user => user.Id,
                    (tutor, user) => new { user, tutor })
                .Where(o => o.user.Role == UserRole.Tutor && o.user.IsDeleted == false)
                //.GroupBy(x => x.)
                .Select(x => new
                {
                    User = x.user,
                    Tutor = x.tutor
                });

        var courseRequestDetailInfo =
            await asyncQueryableExecutor.ToListAsync(courseRequestsQ, false, cancellationToken);

        foreach (var courseRequestInfo in courseRequestDetailInfo)
        {
            var courseRequest = courseFromDb
                .course
                .CourseRequests
                .Where(cr => cr.TutorId == courseRequestInfo.Tutor.Id)
                .Select(x =>
                    (x, courseRequestInfo.User, courseRequestInfo.Tutor).Adapt<CourseRequestListForAdminDto>());
            classDto.CourseRequestListForAdminDtos.AddRange(courseRequest);
        }


        if (courseFromDb.course.TutorId is not null)
        {
            var tutorQ =
                from tutor in tutorRepository.GetAll()
                join user in userRepository.GetAll() on tutor.UserId equals user.Id
                where tutor.Id == courseFromDb.course.TutorId
                select user;

            var tutorInfo = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorQ, false, cancellationToken);

            if (tutorInfo is null)
            {
                return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
            }

            classDto.TutorName = tutorInfo.GetFullName();
            classDto.TutorEmail = tutorInfo.Email;
            classDto.TutorPhoneNumber = tutorInfo.PhoneNumber;
        }

        return classDto;
    }
}