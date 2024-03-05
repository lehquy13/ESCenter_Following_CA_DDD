using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public record GetLearningCourseDetailQuery(Guid CourseId, Guid LearnerId) : IQueryRequest<CourseForDetailDto>;