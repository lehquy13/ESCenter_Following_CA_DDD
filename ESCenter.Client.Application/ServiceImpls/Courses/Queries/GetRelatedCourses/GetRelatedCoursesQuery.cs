using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetRelatedCourses;

public record GetRelatedCoursesQuery(Guid CourseId) : IQueryRequest<IEnumerable<CourseForListDto>>;