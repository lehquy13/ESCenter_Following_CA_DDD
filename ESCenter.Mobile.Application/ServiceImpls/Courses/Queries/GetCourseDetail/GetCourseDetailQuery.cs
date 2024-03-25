using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Queries.GetCourseDetail;

public record GetCourseDetailQuery(Guid CourseId) : IQueryRequest<CourseDetailDto>;