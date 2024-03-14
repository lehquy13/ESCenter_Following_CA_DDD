using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.Contracts.Courses.Params;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourses;

public record GetCoursesQuery(CourseParams CourseParams) : IQueryRequest<PaginatedList<CourseForListDto>>;