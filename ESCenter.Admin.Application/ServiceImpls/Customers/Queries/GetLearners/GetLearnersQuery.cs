using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearners;

public record GetLearnersQuery : IQueryRequest<List<UserForListDto>>;