using ESCenter.Application.Accounts;
using ESCenter.Client.Application.ServiceImpls.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Primitives;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetTutorMajors;

public record GetTutorMajorsQuery() : IQueryRequest<IEnumerable<SubjectMajorDto>>, IAuthorizationRequest;

public class SubjectMajorDto : EntityDto<int>
{
    public string Name { get; init; } = string.Empty;
    public bool IsSelected { get; init; }
}