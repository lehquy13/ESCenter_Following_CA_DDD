using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;

public record GetTutorDetailQuery(Guid TutorId) : IQueryRequest<TutorDetailForClientDto>;

public class GetTutorDetailQueryValidator : AbstractValidator<GetTutorDetailQuery>
{
    public GetTutorDetailQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}
