using ESCenter.Application.Contracts.Users.BasicUsers;
using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutorsForManagement;

public record GetAllTutorsQuery() : IQueryRequest<IEnumerable<TutorListDto>>;