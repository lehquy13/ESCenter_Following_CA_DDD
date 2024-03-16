using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public record GetLearningCourseDetailQuery(Guid CourseId) : IQueryRequest<CourseDetailDto>, IAuthorizationRequest;