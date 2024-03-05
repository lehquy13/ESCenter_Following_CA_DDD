using ESCenter.Admin.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Users.Queries.GetLearnerDetail;

public record GetLearnerDetail(Guid Id) : IQueryRequest<LearnerForCreateUpdateDto>;