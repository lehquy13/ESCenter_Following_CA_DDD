using ESCenter.Client.Application.Contracts.Users.Tutors;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;

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
            join tutor in tutorRepository.GetAll() on user.Id equals tutor.CustomerId
            join course in courseRepository.GetAll().Where(x => x.Review != null) on tutor.Id equals course.TutorId into
                groupCourse
            where tutor.Id == TutorId.Create(request.TutorId)
            select new
            {
                User = user,
                Tutor = tutor,
                Reviews = groupCourse.Where(x => x.Review != null).Select(x => new { x.Review, x.LearnerName })
            };

        var queryResult =
            await asyncQueryableExecutor.FirstOrDefaultAsync(tutorDetailAsQueryable, false, cancellationToken);

        if (queryResult is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var tutorForDetailDto =
            (queryResult.Tutor, queryResult.User).Adapt<TutorDetailForClientDto>();

        tutorForDetailDto.Reviews = queryResult.Reviews.Select(x =>
            new ReviewDto
            {
                LearnerName = x.LearnerName,
                Detail = x.Review.Detail,
                Rate = x.Review.Rate,
                CreationTime = x.Review.CreationTime,
                LastModificationTime = x.Review.LastModificationTime
            }).ToList();

        return tutorForDetailDto;
    }
}