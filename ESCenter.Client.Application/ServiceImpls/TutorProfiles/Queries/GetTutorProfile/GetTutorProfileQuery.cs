using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;

public record GetTutorProfileQuery() : IQueryRequest<TutorForProfileDto>, IAuthorizationRequest;