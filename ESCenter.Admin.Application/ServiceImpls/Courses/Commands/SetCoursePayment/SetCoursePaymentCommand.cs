using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.SetCoursePayment;

// TODO: UI admin
public record SetCoursePaymentCommand(Guid CourseId, Guid TutorId) : ICommandRequest;