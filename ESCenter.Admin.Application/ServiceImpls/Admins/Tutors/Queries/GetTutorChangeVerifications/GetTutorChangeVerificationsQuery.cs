using ESCenter.Admin.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorChangeVerifications;

public record GetTutorChangeVerificationsQuery(Guid TutorId) : IQueryRequest<VerificationEditDto>;