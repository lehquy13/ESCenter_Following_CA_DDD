using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CancelCourseRequest;

public record CancelCourseRequestCommand(
    Guid CourseId,
    Guid CourseRequestId,
    string Description = "Course request has been canceled.") : ICommandRequest;

public class CancelCourseRequestCommandValidator : AbstractValidator<CancelCourseRequestCommand>
{
    public CancelCourseRequestCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.CourseRequestId).NotEmpty();
    }
}
