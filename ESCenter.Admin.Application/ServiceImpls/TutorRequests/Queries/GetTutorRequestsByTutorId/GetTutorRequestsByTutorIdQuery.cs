using ESCenter.Admin.Application.Contracts.Users.Learners;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.TutorRequests.Queries.GetTutorRequestsByTutorId;

public record GetTutorRequestsByTutorIdQuery(Guid TutorId) : IQueryRequest<List<TutorRequestForListDto>>;

public class GetTutorRequestQueryValidator : AbstractValidator<GetTutorRequestsByTutorIdQuery>
{
    public GetTutorRequestQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}