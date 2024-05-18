using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public record GetCourseRequestDetailQuery(Guid CourseId ) : IQueryRequest<CourseRequestForDetailDto>;