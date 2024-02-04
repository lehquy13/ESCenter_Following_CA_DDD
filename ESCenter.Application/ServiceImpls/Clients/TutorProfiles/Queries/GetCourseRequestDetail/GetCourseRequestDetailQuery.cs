using ESCenter.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetCourseRequestDetail;

public record GetCourseRequestDetailQuery(Guid CourseRequestId) : IQueryRequest<CourseRequestForDetailDto>;