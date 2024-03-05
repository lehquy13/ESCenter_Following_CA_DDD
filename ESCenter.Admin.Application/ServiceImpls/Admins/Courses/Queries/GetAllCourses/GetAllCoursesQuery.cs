using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Courses.Queries.GetAllCourses;

public record GetAllCoursesQuery(Status Status) : IQueryRequest<IEnumerable<CourseForListDto>>;