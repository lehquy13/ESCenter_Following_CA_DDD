using ESCenter.Application.Contract.Users.Tutors;
using ESCenter.Application.ServiceImpls.Clients.Tutors.Queries;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.GetTutorProfile;

public record GetTutorProfileQuery() : IQueryRequest<TutorForProfileDto>;