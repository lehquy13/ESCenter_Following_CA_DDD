using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public record GetCourseRequestDetailQuery(Guid CourseRequestId) : IQueryRequest<CourseRequestForDetailDto>;