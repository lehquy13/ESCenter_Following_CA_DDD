using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetCourseRequests;

public record GetCourseRequestsByTutorIdQuery() : IQueryRequest<IEnumerable<CourseRequestForListDto>>;