using ESCenter.Admin.Application.Contracts.Users.Tutors;
using ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearners;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutorsForManagement;

public class GetAllTutorsQueryHandler(
    ITutorRepository tutorRepository,
    ICustomerRepository customerRepository,
    ITutorRequestRepository tutorRequestRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetAllTutorsQuery, IEnumerable<TutorListDto>>(logger, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Result<IEnumerable<TutorListDto>>> Handle(GetAllTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutorQ =
            from userR in customerRepository.GetAll()
            join tutorR in tutorRepository.GetAll() on userR.Id equals tutorR.CustomerId
            select new TutorListDto()
            {
                Id = userR.Id.Value,
                FirstName = userR.FirstName,
                LastName = userR.LastName,
                PhoneNumber = userR.PhoneNumber,
                University = tutorR.University,
                IsVerified = tutorR.IsVerified,
                NumberOfChangeRequests = tutorR.ChangeVerificationRequest == null ? 0 : 1,
                NumberOfRequests = 0
            };

        var tutors = await asyncQueryableExecutor.ToListAsSplitAsync(tutorQ, false, cancellationToken);

        return tutors;
    }
}