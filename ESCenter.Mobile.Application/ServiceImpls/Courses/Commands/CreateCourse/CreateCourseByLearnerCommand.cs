using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourse;

public record CreateCourseByLearnerCommand(CourseForLearnerCreateDto CourseForLearnerCreateDto) : ICommandRequest;

public class CreateCourseByLearnerCommandValidator : AbstractValidator<CreateCourseByLearnerCommand>
{
    public CreateCourseByLearnerCommandValidator()
    {
        RuleFor(x => x.CourseForLearnerCreateDto).NotNull();
        RuleFor(x => x.CourseForLearnerCreateDto).SetValidator(new CourseForLearnerCreateDtoValidator());
    }
}
