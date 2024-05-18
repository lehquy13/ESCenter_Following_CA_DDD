using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.ServiceImpls.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public class GetCourseRequestDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    IAppLogger<GetCourseRequestDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCourseRequestDetailQuery, CourseRequestForDetailDto>(logger, mapper)
{
    public override async Task<Result<CourseRequestForDetailDto>> Handle(GetCourseRequestDetailQuery request,
        CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(CourseId.Create(request.CourseId), cancellationToken);

        if (course == null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var subject = await subjectRepository.GetAsync(course.SubjectId, cancellationToken);

        if (subject == null)
        {
            return Result.Fail(CourseAppServiceErrors.NonExistSubjectError);
        }

        var tutor = await tutorRepository.GetTutorByUserId(CustomerId.Create(currentUserService.UserId),
            cancellationToken);

        if (tutor == null)
        {
            return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
        }

        var courseRequest = course.CourseRequests.FirstOrDefault(x => x.TutorId == tutor.Id);

        // TODO: Check if the mapper is working
        var courseRequestDtos = new CourseRequestForDetailDto()
        {
            Id = course.Id.Value,
            TutorId = tutor.Id.Value,
            CourseId = course.Id.Value,
            Title = course.Title,
            SubjectName = subject.Name,
            Description = course.Description,
            LearnerName = course.LearnerName,
            LearnerContact = course.ContactNumber
        };

        if (courseRequest is null)
        {
            if (course.TutorId != tutor.Id)
            {
                return Result.Fail(CourseAppServiceErrors.NonExistCourseRequestError);
            }

            courseRequestDtos.RequestStatus = RequestStatus.Done.ToString();
        }

        return courseRequestDtos;
    }
}