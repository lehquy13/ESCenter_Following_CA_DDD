using ESCenter.Application.Contract.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetAllCourses;

public record GetAllCoursesQuery() : IQueryRequest<IEnumerable<CourseForListDto>>;