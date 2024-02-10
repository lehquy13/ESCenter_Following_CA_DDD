using ESCenter.Application.Contracts.Users.BasicUsers;
using ESCenter.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearnerDetail;

public record GetLearnerDetail(Guid Id) : IQueryRequest<LearnerForCreateUpdateDto>;