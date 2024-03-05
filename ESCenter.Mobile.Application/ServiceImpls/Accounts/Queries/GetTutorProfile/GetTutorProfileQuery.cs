using ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Queries.GetTutorProfile;

public record GetTutorProfileQuery() : IQueryRequest<TutorForProfileDto>;