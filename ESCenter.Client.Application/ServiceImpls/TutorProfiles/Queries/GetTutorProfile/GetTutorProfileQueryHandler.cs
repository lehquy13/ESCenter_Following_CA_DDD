using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
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
    IReadOnlyRepository<Course, CourseId> courseRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
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
            new TutorByCustomerIdSpec(CustomerId.Create(currentUserService.UserId)),
            cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var subjects = await subjectRepository.GetListAsync(cancellationToken);

        var rawTutor = new TutorForProfileDto()
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
            rawTutor.ChangeVerificationRequestDtos =
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

        var courseRequestsQuery = courseRepository
            .GetAll()
            .Where(c => c.TutorId == tutor.Id)
            .Join(subjectRepository.GetAll(),
                c => c.SubjectId,
                s => s.Id,
                (course, subject) => new { course, subject })
            .Select(x => new { x.course.CourseRequests, x.course.Title, x.subject.Name });

        var firstAndLastName =
            await asyncQueryableExecutor.FirstOrDefaultAsync(firstAndLastNameQuery, false, cancellationToken);
        var courseTitleAndRequests =
            await asyncQueryableExecutor.ToListAsync(courseRequestsQuery, false, cancellationToken);

        // update tutor's course requests
        courseTitleAndRequests.ForEach(c =>
        {
            var title = c.Title;
            var subjectName = c.Name;
            var basicCrDto = c.CourseRequests.Select(cr =>
            {
                var basicCourseRequestDto = new BasicCourseRequestDto()
                {
                    CourseId = cr.CourseId.Value,
                    SubjectName = subjectName,
                    Title = title,
                    Id = cr.Id.Value,
                    RequestStatus = cr.RequestStatus.ToString(),
                    CreationTime = cr.CreationTime,
                    LastModificationTime = cr.LastModificationTime
                };

                return basicCourseRequestDto;
            }).ToList();

            rawTutor.BasicCourseRequests = basicCrDto;
        });

        // update first and last name
        rawTutor.FirstName = firstAndLastName?.FirstName ?? string.Empty;
        rawTutor.LastName = firstAndLastName?.LastName ?? string.Empty;
        rawTutor.Description = firstAndLastName?.Description ?? string.Empty;

        return rawTutor;
    }
}