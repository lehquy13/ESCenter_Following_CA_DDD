using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.Contracts.Courses.Params;
using Matt.Paginated;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.Courses.Queries.GetCourses;

public record GetCoursesQuery(CourseParams CourseParams) : IQueryRequest<PaginatedList<CourseForListDto>>;