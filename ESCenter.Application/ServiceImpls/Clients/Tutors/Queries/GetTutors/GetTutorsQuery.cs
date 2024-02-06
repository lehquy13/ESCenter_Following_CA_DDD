using ESCenter.Application.Contracts.Users.Params;
using ESCenter.Application.Contracts.Users.Tutors;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.Tutors.Queries.GetTutors;

public record GetTutorsQuery(TutorParams TutorParams) : IQueryRequest<PaginatedList<TutorListForClientPageDto>>;