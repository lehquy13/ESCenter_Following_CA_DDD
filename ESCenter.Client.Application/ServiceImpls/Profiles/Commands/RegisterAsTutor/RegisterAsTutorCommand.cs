using ESCenter.Client.Application.Contracts.Users.Tutors;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;

public record RegisterAsTutorCommand(TutorRegistrationDto TutorRegistrationDto) : ICommandRequest;

public class TutorRegistrationDto
{
    public string AcademicLevel { get; init; } = "Student";
    public string University { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<int> MajorIds { get; init; } = [];

    public List<string> ImageFileUrls { get; init; } =
    [
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686459899/z4335058816137_8c84fd04f87afc35b461e273003c7dc3.jpg",
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686723383/Screenshot2023-04-19100710.png"
    ];
}

public class TutorBasicForRegisterCommandValidator : AbstractValidator<TutorRegistrationDto>
{
    public TutorBasicForRegisterCommandValidator()
    {
        RuleFor(x => x.AcademicLevel)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Academic level must be a valid option.");

        RuleFor(x => x.University)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("University name must be between 1 and 100 characters long.");

        // Optional validation for Majors list (you can adjust based on your needs)
        RuleForEach(x => x.MajorIds)
            .NotEmpty()
            .WithMessage("Each major must be between 1 and 10 characters long.");

        // Optional validation for ImageFileUrls list (you can adjust based on your needs)
        RuleForEach(x => x.ImageFileUrls)
            .NotEmpty()
            //.Uri()
            .WithMessage("Please enter a valid image URL.");
    }
}
