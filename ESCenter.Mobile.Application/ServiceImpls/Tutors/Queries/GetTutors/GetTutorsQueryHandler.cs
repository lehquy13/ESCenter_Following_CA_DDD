using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using MapsterMapper;
using Matt.Paginated;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutors;

public class GetTutorsQueryHandler(
    ICurrentUserService currentUserService,
    ITutorRepository tutorRepository,
    ICustomerRepository customerRepository,
    IDiscoveryRepository discoveryRepository,
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetTutorsQueryHandler> logger,
    IMapper mapper
) : QueryHandlerBase<GetTutorsQuery, PaginatedList<TutorListForClientPageDto>>(logger, mapper)
{
    public override async Task<Result<PaginatedList<TutorListForClientPageDto>>> Handle(GetTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutors =
            from tutor in tutorRepository.GetAll()
            join user in customerRepository.GetAll() on tutor.CustomerId equals user.Id
            join course in courseRepository.GetAll() on tutor.Id equals course.TutorId into
                groupCourse
            where tutor.IsVerified == true
            //orderby tutor.Rate, groupCourse.Count() descending
            select new
            {
                Tutor = tutor,
                tutor.TutorMajors,
                User = user,
                Courses = groupCourse
            };

        if (request.TutorParams.Academic?.ToEnum<AcademicLevel>()
                is { } ac && ac != AcademicLevel.Optional)
            tutors = tutors.Where(record => record.User != null && record.Tutor.AcademicLevel == ac);

        if (!string.IsNullOrEmpty(request.TutorParams.Address))
            tutors = tutors.Where(record => record.User.Address.Match(request.TutorParams.Address));

        if (request.TutorParams.Gender is { } gender && gender != GenderEnum.None)
            tutors = tutors.Where(record => record.User.Gender == gender.ToEnum<Gender>());

        if (request.TutorParams.BirthYear != 0)
            tutors = tutors.Where(record => record.User.BirthYear == request.TutorParams.BirthYear);

        if (!string.IsNullOrEmpty(request.TutorParams.SubjectName))
            tutors = tutors.Where(record =>
                record.Tutor.TutorMajors.Any(sub =>
                    sub.SubjectName.ToLower().Contains(request.TutorParams.SubjectName.ToLower()))
            );

        var totalCount = await asyncQueryableExecutor.LongCountAsync(tutors, cancellationToken);

        if (currentUserService.IsAuthenticated)
        {
            var userGuid = CustomerId.Create(currentUserService.UserId);
            var discoveryQueryable = await discoveryRepository.GetUserDiscoverySubjects(userGuid, cancellationToken);

            var learntSubjectQueryable =
                from userForSearch in customerRepository.GetAll()
                join courseForSearch in courseRepository.GetAll() on userForSearch.Id equals courseForSearch
                    .LearnerId
                where userForSearch.Id == userGuid
                select courseForSearch.SubjectId;

            // Order by the number of subjects that the user has discovered
            tutors = tutors
                .OrderByDescending(record => record.Tutor.Rate)
                .ThenByDescending(record => record.Courses.Count())
                .ThenByDescending(record => record.TutorMajors.Join(
                    discoveryQueryable,
                    tutorMajor => tutorMajor.SubjectId,
                    discovery => discovery,
                    (tutor, discovery) => tutor).Count())
                .ThenByDescending(record => record.TutorMajors.Join(
                    learntSubjectQueryable,
                    tutorMajor => tutorMajor.SubjectId,
                    learntSubject => learntSubject,
                    (tutorMajor, learntSubject) => tutorMajor).Count());
        }
        else
        {
            tutors = tutors.OrderByDescending(record => record.Tutor.Rate)
                .ThenByDescending(record => record.Courses.Count());
        }

        var tutorFromDb = await asyncQueryableExecutor
            .ToListAsSplitAsync(tutors
                    .Skip((request.TutorParams.PageIndex - 1) * request.TutorParams.PageSize)
                    .Take(request.TutorParams.PageSize),
                false, cancellationToken);

        var mergeList = tutorFromDb.Select(
            x => new TutorListForClientPageDto()
            {
                Id = x.Tutor.Id.Value,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                BirthYear = x.User.BirthYear,
                Description = x.User.Description,
                Avatar = x.User.Avatar,
                AcademicLevel = x.Tutor.AcademicLevel.ToString(),
                University = x.Tutor.University,
                Rate = x.Tutor.Rate
            }
        );

        var result = PaginatedList<TutorListForClientPageDto>
            .Create(
                mergeList,
                request.TutorParams.PageIndex,
                request.TutorParams.PageSize,
                (int)totalCount);

        return result;
    }
}