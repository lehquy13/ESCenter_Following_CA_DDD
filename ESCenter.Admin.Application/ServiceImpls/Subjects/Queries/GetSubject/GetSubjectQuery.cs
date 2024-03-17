using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubject;

public record GetSubjectQuery(int Id) : IQueryRequest<SubjectDto>;

public class GetSubjectQueryValidator : AbstractValidator<GetSubjectQuery>
{
    public GetSubjectQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
