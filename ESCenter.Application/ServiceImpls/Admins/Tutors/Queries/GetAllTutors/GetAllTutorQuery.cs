using ESCenter.Application.Contract.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutors;

public record GetAllTutorQuery() : IQueryRequest<List<UserForListDto>>;