using ESCenter.Admin.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Queries.GetStaff;

public record GetStaffQuery(Guid Id) : IQueryRequest<LearnerForCreateUpdateDto>, IAuthorizationRequest;