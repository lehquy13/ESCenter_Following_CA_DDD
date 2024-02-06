using ESCenter.Application.Contract.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetVerification;

public record GetVerificationsByTutorIdQuery(Guid TutorId) : IQueryRequest<TutorVerificationInfoForEditDto>;