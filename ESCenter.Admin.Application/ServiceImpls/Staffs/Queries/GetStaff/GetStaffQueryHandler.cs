using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Queries.GetStaff;

public class GetStaffQueryHandler(
    IReadOnlyRepository<Customer, CustomerId> customerRepository,
    IAppLogger<GetStaffQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetStaffQuery, LearnerForCreateUpdateDto>(logger, mapper)
{
    public override async Task<Result<LearnerForCreateUpdateDto>> Handle(GetStaffQuery query,
        CancellationToken cancellationToken)
    {
        var staff = await
            customerRepository
                .GetAsync(CustomerId.Create(query.Id), cancellationToken);

        if (staff is null)
        {
            return Result.Fail(StaffAppServiceError.StaffNotFound);
        }
        
        return Mapper.Map<LearnerForCreateUpdateDto>(staff);
    }
}