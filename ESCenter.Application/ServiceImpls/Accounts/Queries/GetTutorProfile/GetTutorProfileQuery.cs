using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.GetTutorProfile;

public record GetTutorProfileQuery() : IQueryRequest<TutorForProfileDto>;