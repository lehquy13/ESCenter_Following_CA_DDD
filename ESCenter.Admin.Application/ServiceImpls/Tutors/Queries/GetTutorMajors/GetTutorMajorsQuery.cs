using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorMajors;

public record GetTutorMajorsQuery(Guid TutorId) : IQueryRequest<List<SubjectDto>>;

public class GetTutorMajorsQueryValidator : AbstractValidator<GetTutorMajorsQuery>
{
    public GetTutorMajorsQueryValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
    }
}