using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;

public record GetTutorProfileQuery(Guid TutorId) : IQueryRequest<TutorMinimalBasicDto>;