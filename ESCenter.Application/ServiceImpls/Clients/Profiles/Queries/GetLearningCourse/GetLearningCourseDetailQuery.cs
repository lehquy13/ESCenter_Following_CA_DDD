using ESCenter.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.Profiles.Queries.GetLearningCourse;

public record GetLearningCourseDetailQuery(Guid CourseId, Guid LearnerId) : IQueryRequest<CourseForDetailDto>;