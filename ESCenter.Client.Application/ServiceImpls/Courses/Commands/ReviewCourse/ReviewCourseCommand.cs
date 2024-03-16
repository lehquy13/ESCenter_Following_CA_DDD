using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public record ReviewCourseCommand(Guid CourseId, string Detail, short Rate) : ICommandRequest, IAuthorizationRequest;