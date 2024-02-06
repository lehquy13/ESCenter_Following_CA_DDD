using ESCenter.Application.Contract.Users.Params;
using ESCenter.Application.Contract.Users.Tutors;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.Tutors.Queries.GetTutors;

public record GetTutorsQuery(TutorParams TutorParams) : IQueryRequest<PaginatedList<TutorListForClientPageDto>>;