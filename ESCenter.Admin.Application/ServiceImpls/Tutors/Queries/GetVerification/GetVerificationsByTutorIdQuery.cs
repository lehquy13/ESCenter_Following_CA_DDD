using ESCenter.Admin.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetVerification;

public record GetVerificationsByTutorIdQuery(Guid TutorId) : IQueryRequest<VerificationEditDto>;

public class GetVerificationsByTutorIdQueryValidator : AbstractValidator<GetVerificationsByTutorIdQuery>
{
    public GetVerificationsByTutorIdQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}