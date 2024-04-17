using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearners;

public class GetLearnersQueryHandler(
    ICustomerRepository customerRepository,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetLearnersQuery, List<UserForListDto>>(logger, mapper)
{
    public override async Task<Result<List<UserForListDto>>> Handle(GetLearnersQuery request,
        CancellationToken cancellationToken)
    {
        var learnersFromDb = await customerRepository.GetLearners();
        var userForListDtos = Mapper.Map<List<UserForListDto>>(learnersFromDb);
        
        return userForListDtos;
    }
}