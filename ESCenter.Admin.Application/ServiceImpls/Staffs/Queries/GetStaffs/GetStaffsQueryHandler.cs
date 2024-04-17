using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Queries.GetStaffs;

public class GetStaffsQueryHandler(
    IReadOnlyRepository<Customer, CustomerId> customerRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetStaffsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetStaffsQuery, IEnumerable<UserForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<UserForListDto>>> Handle(GetStaffsQuery query,
        CancellationToken cancellationToken)
    {
        var staffsQueryable = customerRepository.GetAll().Where(x => x.Role == Role.Admin);
                
        var staffs = await asyncQueryableExecutor.ToListAsync(staffsQueryable, false, cancellationToken);

        return Mapper.Map<List<UserForListDto>>(staffs);
    }
}