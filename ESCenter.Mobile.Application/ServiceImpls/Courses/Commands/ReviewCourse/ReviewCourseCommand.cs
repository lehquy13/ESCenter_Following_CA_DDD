using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public record ReviewCourseCommand(Guid CourseId, short Rate, string Detail) : ICommandRequest, IAuthorizationRequest;

public class ReviewCourseCommandValidator : AbstractValidator<ReviewCourseCommand>
{
    public ReviewCourseCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.Rate).InclusiveBetween((short)1, (short)5);
        RuleFor(x => x.Detail).NotEmpty();
    }
}

public class ReviewCreateViewModel
{
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}