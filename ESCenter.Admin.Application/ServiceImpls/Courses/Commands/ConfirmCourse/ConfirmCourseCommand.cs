using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.ConfirmCourse;

public record ConfirmCourseCommand(Guid CourseId) : ICommandRequest;

// To confirm a course, the course must exist and the payment must be confirmed