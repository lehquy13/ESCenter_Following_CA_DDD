using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Courses.Queries.GetCourseRequest;

public record GetCourseRequestQuery(Guid CourseRequestId) : IQueryRequest<CourseRequestCancelDto>;