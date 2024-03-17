using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.Accounts.Queries.ValidateToken;

public record ValidateTokenQuery(
    string ValidateToken
) : IQueryRequest;

public class ValidateTokenQueryValidator : AbstractValidator<ValidateTokenQuery>
{
    public ValidateTokenQueryValidator()
    {
        RuleFor(x => x.ValidateToken)
            .NotEmpty()
            .WithMessage("Please enter a valid token.");
    }
}
