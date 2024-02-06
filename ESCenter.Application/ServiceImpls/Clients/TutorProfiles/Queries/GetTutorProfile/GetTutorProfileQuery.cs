using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetTutorProfile;

public record GetTutorProfileQuery(Guid TutorId) : IQueryRequest<TutorMinimalBasicDto>;