using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;

public record GetTutorProfileQuery(Guid TutorId) : IQueryRequest<TutorMinimalBasicDto>;