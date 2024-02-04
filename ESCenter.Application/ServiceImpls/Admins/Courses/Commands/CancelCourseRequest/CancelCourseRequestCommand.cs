using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Commands.CancelCourseRequest;

public record CancelCourseRequestCommand(Guid CourseRequestId) : ICommandRequest;