using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Specifications.Tutors;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;

public class GetTutorProfileQueryHandler(
    ITutorRepository tutorRepository,
    IReadOnlyRepository<Customer, CustomerId> customerRepository,
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ICurrentUserService currentUserService,
    IMapper mapper,
    IAppLogger<GetTutorProfileQueryHandler> logger)
    : QueryHandlerBase<GetTutorProfileQuery, TutorForProfileDto>(logger, mapper)
{
    public override async Task<Result<TutorForProfileDto>> Handle(GetTutorProfileQuery request,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(
            new TutorByCustomerIdSpec(CustomerId.Create(currentUserService.UserId)), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var subjects = await subjectRepository.GetAllListAsync(cancellationToken);

        var tutorProfile = new TutorForProfileDto()
        {
            University = tutor.University,
            AcademicLevel = tutor.AcademicLevel.ToString(),
            IsVerified = tutor.IsVerified,
            Rate = tutor.Rate,
            VerificationDtos = tutor.Verifications
                .Select(v =>
                    new VerificationDto
                    {
                        Id = v.Id.Value,
                        Image = v.Image,
                        //CreationTime = v.CreationTime,
                        //LastModificationTime = v.LastModificationTime
                    })
                .ToList(),
            Majors = subjects.Select(x => new TutorMajorDto()
            {
                IsMajored = tutor.TutorMajors.Any(tm => tm.SubjectId == x.Id),
                SubjectId = x.Id.Value,
                SubjectName = x.Name
            }).ToList()
        };

        if (tutor.ChangeVerificationRequest is not null)
        {
            tutorProfile.ChangeVerificationRequestDtos =
            [
                new ChangeVerificationRequestDto
                {
                    Id = tutor.ChangeVerificationRequest.Id.Value,
                    RequestStatus = tutor.ChangeVerificationRequest.RequestStatus.ToString(),
                    ChangeVerificationRequestDetails = tutor.ChangeVerificationRequest
                        .ChangeVerificationRequestDetails
                        .Select(x => x.ImageUrl)
                        .ToList()
                }
            ];
        }

        var firstAndLastNameQuery = customerRepository
            .GetAll()
            .Where(c => c.Id == tutor.CustomerId)
            .Select(c => new { c.FirstName, c.LastName, c.Description });
        
        var firstAndLastName =
            await asyncQueryableExecutor.FirstOrDefaultAsync(firstAndLastNameQuery, false, cancellationToken);

        // update first and last name of 
        tutorProfile.FirstName = firstAndLastName?.FirstName ?? string.Empty;
        tutorProfile.LastName = firstAndLastName?.LastName ?? string.Empty;
        tutorProfile.Description = firstAndLastName?.Description ?? string.Empty;

        // Get the course that have tutor id equal to the tutor id
        var allTutorRelatedCourses = await courseRepository.GetAllTutorRelatedCourses(tutor.Id);
        var courseRequestsQuery = allTutorRelatedCourses 
            .SelectMany(x => x.CourseRequests)
            .Where(x => x.TutorId == tutor.Id)
            .ToList();
        
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

            tutorProfile.BasicCourseRequests.Add(basicCourseRequestDto);
        });

        return tutorProfile;
    }
}