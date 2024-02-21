using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorDetail;

public record GetTutorDetailQuery(Guid TutorId) : IQueryRequest<TutorUpdateDto>;