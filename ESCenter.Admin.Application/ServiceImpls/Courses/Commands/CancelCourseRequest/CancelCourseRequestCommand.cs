using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CancelCourseRequest;

public record CancelCourseRequestCommand(
    Guid CourseRequestId,
    Guid CourseId,
    string Description = "Course request has been canceled.") : ICommandRequest;

public class CancelCourseRequestCommandValidator : AbstractValidator<CancelCourseRequestCommand>
{
    public CancelCourseRequestCommandValidator()
    {
        RuleFor(x => x.CourseRequestId).NotEmpty();
        RuleFor(x => x.CourseId).NotEmpty();
    }
}
