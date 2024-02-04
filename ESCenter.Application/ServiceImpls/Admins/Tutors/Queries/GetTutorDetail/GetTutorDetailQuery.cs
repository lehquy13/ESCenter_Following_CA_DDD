using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorDetail;

/// <summary>
/// Deprecated
/// </summary>
/// <param name="TutorId"></param>
public record GetTutorDetailQuery(Guid TutorId) : IQueryRequest<TutorForDetailDto>;