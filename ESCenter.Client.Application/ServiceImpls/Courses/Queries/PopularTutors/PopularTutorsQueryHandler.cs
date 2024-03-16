using ESCenter.Client.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.PopularTutors;

public class PopularTutorsQueryHandler(
    ITutorRepository tutorRepository,
    IUserRepository userRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : QueryHandlerBase<PopularTutorsQuery, IEnumerable<TutorListForClientPageDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<IEnumerable<TutorListForClientPageDto>>> Handle(PopularTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutorsQueryable =
            from tutor in tutorRepository.GetAll()
            join user in userRepository.GetAll() on tutor.UserId equals user.Id
            where tutor.IsVerified == true
            select new
            {
                User = user,
                Tutor = tutor
            };

        var tutors = await asyncQueryableExecutor.ToListAsync(tutorsQueryable, false, cancellationToken);

        var tutorListForClientPageDtos = tutors
            .Select(x => (x.User, x.Tutor).Adapt<TutorListForClientPageDto>())
            .ToList();

        return tutorListForClientPageDtos;
    }
}