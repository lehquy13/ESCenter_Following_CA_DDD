using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseDetail;

public record GetCourseDetailQuery(Guid CourseId) : IQueryRequest<CourseForDetailDto>;

public class GetCourseDetailQueryValidator : AbstractValidator<GetCourseDetailQuery>
{
    public GetCourseDetailQueryValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}