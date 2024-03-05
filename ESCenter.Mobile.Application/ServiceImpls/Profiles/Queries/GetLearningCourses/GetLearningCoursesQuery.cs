using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourses;

public record GetLearningCoursesQuery() : IQueryRequest<IEnumerable<LearningCourseForListDto>>;