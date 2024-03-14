using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseDetail;

public record GetCourseDetailQuery(Guid CourseId) : IQueryRequest<CourseForDetailDto>;