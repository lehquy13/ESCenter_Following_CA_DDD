using ESCenter.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearnerDetail;

public record GetLearnerDetail(Guid Id) : IQueryRequest<UserForDetailDto>;