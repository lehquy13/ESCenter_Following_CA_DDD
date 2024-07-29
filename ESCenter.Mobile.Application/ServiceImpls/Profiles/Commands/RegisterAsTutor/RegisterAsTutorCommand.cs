using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;

public record RegisterAsTutorCommand(TutorRegistrationDto TutorRegistrationDto) : ICommandRequest, IAuthorizationRequest;

public class TutorRegistrationDto
{
    public string AcademicLevel { get; init; } = "Student";
    public string University { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<int> Majors { get; init; } = [];

    public List<string> ImageFileUrls { get; init; } =
    [
        "https://firebasestorage.googleapis.com/v0/b/eds-storage.appspot.com/o/images%2F9e4860b3-756e-42c0-ae56-359f9d533b49.6.16.TH.ns2.jpg?alt=media&token=ca96e7d4-95c8-4099-9ae0-bb7c922089ad",
        "https://firebasestorage.googleapis.com/v0/b/eds-storage.appspot.com/o/images%2F5b_NHDG.jpg?alt=media&token=3779da91-4699-4aaf-a3eb-57e8fda7ca4a"
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
        RuleForEach(x => x.Majors)
            .NotEmpty()
            .WithMessage("Each major must be between 1 and 10 characters long.");

        // Optional validation for ImageFileUrls list (you can adjust based on your needs)
        RuleForEach(x => x.ImageFileUrls)
            .NotEmpty()
            //.Uri()
            .WithMessage("Please enter a valid image URL.");
    }
}