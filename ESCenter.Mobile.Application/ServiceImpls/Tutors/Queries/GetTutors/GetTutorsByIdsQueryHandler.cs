using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutors;

public record GetTutorsByIdsQuery(List<Guid> Guids) : IQueryRequest<IEnumerable<TutorListForClientPageDto>>;

public class GetTutorsByIdsQueryHandler(
    ITutorRepository tutorRepository,
    ICustomerRepository customerRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetTutorsQueryHandler> logger,
    IMapper mapper
) : QueryHandlerBase<GetTutorsByIdsQuery, IEnumerable<TutorListForClientPageDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<TutorListForClientPageDto>>> Handle(GetTutorsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        var tutorIds = request.Guids.Select(TutorId.Create);

        var tutors =
            from tutor in tutorRepository.GetAll().Where(x => tutorIds.Contains(x.Id))
            join user in customerRepository.GetAll() on tutor.CustomerId equals user.Id
            where tutor.IsVerified == true
            select new
            {
                Tutor = tutor,
                tutor.TutorMajors,
                User = user
            };

        var tutorFromDb = await asyncQueryableExecutor.ToListAsync(tutors, false, cancellationToken);

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

        return Result<IEnumerable<TutorListForClientPageDto>>.Success(mergeList);
    }
}