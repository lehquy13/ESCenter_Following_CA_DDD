using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;

public record CreateCourseRequestCommand(CourseRequestForCreateDto CourseRequestForCreateDto) : ICommandRequest, IAuthorizationRequest;

public class CreateCourseRequestCommandValidator : AbstractValidator<CreateCourseRequestCommand>
{
    public CreateCourseRequestCommandValidator()
    {
        RuleFor(x => x.CourseRequestForCreateDto).NotNull();
        RuleFor(x => x.CourseRequestForCreateDto).SetValidator(new CourseRequestForCreateDtoValidator());
    }
}

public class CourseRequestForCreateDto
{
    public Guid CourseId { get; set; }
    public Guid TutorId { get; set; }
}

public class CourseRequestForCreateDtoValidator : AbstractValidator<CourseRequestForCreateDto>
{
    public CourseRequestForCreateDtoValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.TutorId).NotEmpty();
    }
}