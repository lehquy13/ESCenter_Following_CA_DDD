using ESCenter.Admin.Application.Contracts.Users.Learners;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorRequests;

public record GetTutorRequestQuery(Guid TutorId) : IQueryRequest<List<TutorRequestForListDto>>;

public class GetTutorRequestQueryValidator : AbstractValidator<GetTutorRequestQuery>
{
    public GetTutorRequestQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}