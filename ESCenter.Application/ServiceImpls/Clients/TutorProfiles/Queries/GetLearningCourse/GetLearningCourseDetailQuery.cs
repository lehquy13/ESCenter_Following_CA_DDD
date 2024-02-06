using ESCenter.Application.Contract.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetLearningCourse;

public record GetLearningCourseDetailQuery(Guid CourseId, Guid LearnerId) : IQueryRequest<CourseForDetailDto>;