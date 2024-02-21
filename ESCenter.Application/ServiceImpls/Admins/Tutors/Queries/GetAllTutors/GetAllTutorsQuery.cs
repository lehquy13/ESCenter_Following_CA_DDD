using ESCenter.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutors;

public record GetAllTutorsQuery() : IQueryRequest<List<UserForListDto>>;