using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;

public class GetTutorDetailQueryHandler(
    ICustomerRepository customerRepository,
    ITutorRepository tutorRepository,
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetTutorDetailQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorDetailQuery, TutorDetailForClientDto>(logger, mapper)
{
    public override async Task<Result<TutorDetailForClientDto>> Handle(GetTutorDetailQuery request,
        CancellationToken cancellationToken)
    {
        var tutorDetailAsQueryable =
            from user in customerRepository.GetAll()
            join tutor in tutorRepository.GetAll() on user.Id equals tutor.UserId
            join course in courseRepository.GetAll() on tutor.Id equals course.TutorId into groupCourse
            where user.Id == CustomerId.Create(request.TutorId)
            select new
            {
                User = user,
                Tutor = tutor,
                Reviews = groupCourse.Select(x => new { x.Review, x.LearnerName })
            };

        var queryResult =
            await asyncQueryableExecutor.FirstOrDefaultAsync(tutorDetailAsQueryable, false, cancellationToken);

        if (queryResult is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var tutorForDetailDto =
            (queryResult.Tutor, queryResult.User).Adapt<TutorDetailForClientDto>();

        tutorForDetailDto.Reviews =
            queryResult.Reviews.Select(x => (x.Review, x.LearnerName).Adapt<ReviewDto>()).ToList();


        return tutorForDetailDto;
    }
}