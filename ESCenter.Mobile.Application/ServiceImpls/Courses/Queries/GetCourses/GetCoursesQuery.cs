using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.Contracts.Courses.Params;
using FluentValidation;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Queries.GetCourses;

public record GetCoursesQuery(CourseParams CourseParams) : IQueryRequest<PaginatedList<CourseForListDto>>;

public class GetCoursesQueryValidator : AbstractValidator<GetCoursesQuery>
{
    public GetCoursesQueryValidator()
    {
        RuleFor(x => x.CourseParams).NotNull();
    }
}