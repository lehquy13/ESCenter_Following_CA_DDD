using ESCenter.Admin.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetVerification;

public record GetVerificationsByTutorIdQuery(Guid TutorId) : IQueryRequest<VerificationEditDto>;