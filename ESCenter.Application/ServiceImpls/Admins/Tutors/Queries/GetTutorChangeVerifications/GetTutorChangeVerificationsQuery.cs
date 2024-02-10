using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorChangeVerifications;

public record GetTutorChangeVerificationsQuery(Guid TutorId) : IQueryRequest<TutorVerificationInfoForEditDto>;