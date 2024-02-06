using ESCenter.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Clients.Profiles.Queries.GetLearningCourses;

public record GetLearningCoursesQuery() : IQueryRequest<IEnumerable<LearningCourseForListDto>>;