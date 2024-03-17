using ESCenter.Admin.Application.Contracts.Users.Learners;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Users.Queries.GetLearnerDetail;

public record GetLearnerDetail(Guid Id) : IQueryRequest<LearnerForCreateUpdateDto>;

public class GetLearnerDetailValidator : AbstractValidator<GetLearnerDetail>
{
    public GetLearnerDetailValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}