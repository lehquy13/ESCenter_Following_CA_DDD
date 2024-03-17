using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public record ReviewCourseCommand(ReviewDetailDto ReviewDetailDto) : ICommandRequest, IAuthorizationRequest;

public class ReviewCourseCommandValidator : AbstractValidator<ReviewCourseCommand>
{
    public ReviewCourseCommandValidator()
    {
        RuleFor(x => x.ReviewDetailDto).NotNull();
        RuleFor(x => x.ReviewDetailDto).SetValidator(new ReviewDetailDtoValidator());
    }
}

public class ReviewDetailDto
{
    public Guid CourseId { get; set; }
    public Guid LearnerId { get; set; }
    public string LearnerName { get; set; } = "";
    public short Rate { get; set; } = 5;
    public string Detail { get; set; } = "";
}

public class ReviewDetailDtoValidator : AbstractValidator<ReviewDetailDto>
{
    public ReviewDetailDtoValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.LearnerId).NotEmpty();
        RuleFor(x => x.LearnerName).NotEmpty();
        RuleFor(x => x.Rate).InclusiveBetween((short)1, (short)5);
        RuleFor(x => x.Detail).NotEmpty();
    }
}