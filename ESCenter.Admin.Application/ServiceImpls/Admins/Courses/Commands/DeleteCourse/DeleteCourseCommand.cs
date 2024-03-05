using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Courses.Commands.DeleteCourse;

public record DeleteCourseCommand(Guid Id) : ICommandRequest;