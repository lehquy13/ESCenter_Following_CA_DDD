using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Queries.GetStaffs;

public record GetStaffsQuery() : IQueryRequest<IEnumerable<UserForListDto>>, IAuthorizationRequest;