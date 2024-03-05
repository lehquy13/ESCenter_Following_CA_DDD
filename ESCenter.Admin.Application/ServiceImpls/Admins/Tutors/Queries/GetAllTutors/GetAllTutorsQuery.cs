using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutors;

public record GetAllTutorsQuery() : IQueryRequest<List<UserForListDto>>;