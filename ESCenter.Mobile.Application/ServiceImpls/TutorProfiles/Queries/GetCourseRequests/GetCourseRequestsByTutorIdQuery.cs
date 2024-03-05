using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequests;

public record GetCourseRequestsByTutorIdQuery() : IQueryRequest<IEnumerable<CourseRequestForListDto>>;