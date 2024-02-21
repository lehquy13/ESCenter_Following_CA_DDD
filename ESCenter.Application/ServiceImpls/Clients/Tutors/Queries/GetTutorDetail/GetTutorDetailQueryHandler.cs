using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Clients.TutorProfiles;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Clients.Tutors.Queries.GetTutorDetail;

public class GetTutorDetailQueryHandler(
    IUserRepository userRepository,
    ITutorRepository tutorRepository,
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetTutorDetailQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetTutorDetailQuery, TutorDetailForClientDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<TutorDetailForClientDto>> Handle(GetTutorDetailQuery request,
        CancellationToken cancellationToken)
    {
        var tutorDetailAsQueryable =
            from user in userRepository.GetAll()
            join tutor in tutorRepository.GetAll() on user.Id equals tutor.UserId
            join course in courseRepository.GetAll() on tutor.Id equals course.TutorId into groupCourse
            where user.Id == IdentityGuid.Create(request.TutorId)
            select new
            {
                User = user,
                Tutor = tutor,
                Reviews = groupCourse.Select(x => x.Review),
            };

        var queryResult =
            await asyncQueryableExecutor.FirstOrDefaultAsync(tutorDetailAsQueryable, false, cancellationToken);

        if (queryResult is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var tutorForDetailDto = Mapper.Map<TutorDetailForClientDto>(queryResult);

        return tutorForDetailDto;
    }
}