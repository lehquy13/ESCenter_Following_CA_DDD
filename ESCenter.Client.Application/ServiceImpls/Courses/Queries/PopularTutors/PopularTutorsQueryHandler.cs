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
    ICustomerRepository customerRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : QueryHandlerBase<PopularTutorsQuery, IEnumerable<TutorListForClientPageDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<TutorListForClientPageDto>>> Handle(PopularTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutors = await tutorRepository.GetPopularTutors();
        
        var userQueryable =
            from user in customerRepository.GetAll() 
            where tutors.Select(x => x.CustomerId).Contains(user.Id)
            select new
            {
                User = user
            };

        var users = await asyncQueryableExecutor.ToListAsync(userQueryable, false, cancellationToken);

        var joined = tutors.Join(users, tutor => tutor.CustomerId, user => user.User.Id, (tutor, user) => new
        {
            Tutor = tutor,
            User = user.User
        });
        
        var tutorListForClientPageDtos = joined
            .Select(x => (x.User, x.Tutor).Adapt<TutorListForClientPageDto>())
            .ToList();
        
        return tutorListForClientPageDtos;
    }
}