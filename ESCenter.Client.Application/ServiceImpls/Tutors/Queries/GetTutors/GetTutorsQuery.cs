using ESCenter.Client.Application.Contracts.Users.Params;
using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutors;

public record GetTutorsQuery(TutorParams TutorParams) : IQueryRequest<PaginatedList<TutorListForClientPageDto>>;