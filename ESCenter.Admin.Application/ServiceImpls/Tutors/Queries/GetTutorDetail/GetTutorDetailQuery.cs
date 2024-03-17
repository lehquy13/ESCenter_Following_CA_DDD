using ESCenter.Admin.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;

public record GetTutorDetailQuery(Guid TutorId) : IQueryRequest<TutorUpdateDto>;

public class GetTutorDetailQueryValidator : AbstractValidator<GetTutorDetailQuery>
{
    public GetTutorDetailQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}