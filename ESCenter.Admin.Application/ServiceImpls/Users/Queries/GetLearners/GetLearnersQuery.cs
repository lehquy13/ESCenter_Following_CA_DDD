using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Users.Queries.GetLearners;

public record GetLearnersQuery : IQueryRequest<List<UserForListDto>>;