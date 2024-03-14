using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequests;

public record GetCourseRequestsByTutorIdQuery() : IQueryRequest<IEnumerable<CourseRequestForListDto>>;