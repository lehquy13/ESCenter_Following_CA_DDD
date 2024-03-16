using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourseDetail;

public record GetCourseDetailQuery(Guid CourseId) : IQueryRequest<CourseDetailDto>;