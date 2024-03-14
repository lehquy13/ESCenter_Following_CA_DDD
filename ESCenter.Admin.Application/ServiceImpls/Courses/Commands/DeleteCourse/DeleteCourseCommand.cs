using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.DeleteCourse;

public record DeleteCourseCommand(Guid Id) : ICommandRequest;