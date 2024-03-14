using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetTodayCourses;

public record GetTodayCoursesQuery() : IQueryRequest<IEnumerable<CourseForListDto>>;