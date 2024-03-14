using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;

public record GetTutorDetailQuery(Guid TutorId) : IQueryRequest<TutorDetailForClientDto>;