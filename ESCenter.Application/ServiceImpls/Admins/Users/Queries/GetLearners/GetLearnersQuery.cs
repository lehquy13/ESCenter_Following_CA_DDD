using ESCenter.Application.Contract.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearners;

public record GetLearnersQuery : IQueryRequest<List<UserForListDto>>;