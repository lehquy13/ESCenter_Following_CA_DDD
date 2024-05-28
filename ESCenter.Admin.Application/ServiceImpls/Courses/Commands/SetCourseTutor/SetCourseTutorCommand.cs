using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.SetCourseTutor;

public record SetCourseTutorCommand(Guid CourseId, Guid TutorId) : ICommandRequest;