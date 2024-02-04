using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries;

public record GetTutorProfiles(Guid TutorId) : IQueryRequest<TutorMinimalBasicDto>;