using ESCenter.Admin.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorChangeVerifications;

public record GetTutorChangeVerificationsQuery(Guid TutorId) : IQueryRequest<VerificationEditDto>;

public class GetTutorChangeVerificationsQueryValidator : AbstractValidator<GetTutorChangeVerificationsQuery>
{
    public GetTutorChangeVerificationsQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}