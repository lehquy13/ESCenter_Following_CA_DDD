using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.RemoveReview;

public record RemoveCourseReviewCommand(Guid CourseId) : ICommandRequest;

public class RemoveCourseReviewCommandValidator : AbstractValidator<RemoveCourseReviewCommand>
{
    public RemoveCourseReviewCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}