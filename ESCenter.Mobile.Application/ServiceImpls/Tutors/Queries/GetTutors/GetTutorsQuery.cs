using ESCenter.Mobile.Application.Contracts.Users.Params;
using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutors;

public record GetTutorsQuery(TutorParams TutorParams) : IQueryRequest<PaginatedList<TutorListForClientPageDto>>;