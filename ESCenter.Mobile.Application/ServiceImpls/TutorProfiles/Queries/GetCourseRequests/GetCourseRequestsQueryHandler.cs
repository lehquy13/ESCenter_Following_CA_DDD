using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequests;

public class GetCourseRequestsQueryHandler(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    ISubjectRepository subjectRepository,
    ICurrentUserService currentUserService,
    IAppLogger<GetCourseRequestsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestsQuery, IEnumerable<BasicCourseRequestDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<BasicCourseRequestDto>>> Handle(
        GetCourseRequestsQuery requests,
        CancellationToken cancellationToken)
    {
        var customerId = CustomerId.Create(currentUserService.UserId);
        var tutor = await tutorRepository.GetTutorByUserId(customerId, cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var subjects = await subjectRepository.GetListAsync(cancellationToken);

        // Get the course that have tutor id equal to the tutor id
        var allTutorRelatedCourses = await courseRepository.GetAllTutorRelatedCourses(tutor.Id);
        var courseRequestsQuery = allTutorRelatedCourses
            .SelectMany(x => x.CourseRequests)
            .Where(x => x.TutorId == tutor.Id)
            .ToList();

        var courseRequestDtos = new List<BasicCourseRequestDto>();

        // update tutor's course requests
        allTutorRelatedCourses.AsParallel().ForAll(course =>
        {
            var title = course.Title;
            var subjectName = subjects.FirstOrDefault(x => x.Id == course.SubjectId)?.Name ?? string.Empty;

            // Not only course requests, there are also courses that being assigned by administrator
            // Then this query can be used to filter out the right result that we want
            var basicCourseRequestDto = new BasicCourseRequestDto()
            {
                CourseId = course.Id.Value,
                SubjectName = subjectName,
                Title = title,
                Id = course.Id.Value,
                CreationTime = course.CreationTime,
                LastModificationTime = course.LastModificationTime,
            };

            var basicCrDto = courseRequestsQuery
                .FirstOrDefault(x => x.CourseId == course.Id);

            if (basicCrDto is not null)
            {
                basicCourseRequestDto.RequestStatus = basicCrDto.RequestStatus.ToString();
                basicCourseRequestDto.CreationTime = basicCrDto.CreationTime;
                basicCourseRequestDto.LastModificationTime = basicCrDto.LastModificationTime;
            }
            else if (course.TutorId == tutor.Id && course.Status == Status.Confirmed)
            {
                basicCourseRequestDto.RequestStatus = RequestStatus.Done.ToString();
            }

            courseRequestDtos.Add(basicCourseRequestDto);
        });

        return courseRequestDtos;
    }
}