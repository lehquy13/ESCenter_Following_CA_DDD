using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.PopularTutors;

public record PopularTutorsQuery() : IQueryRequest<IEnumerable<TutorListForClientPageDto>>;