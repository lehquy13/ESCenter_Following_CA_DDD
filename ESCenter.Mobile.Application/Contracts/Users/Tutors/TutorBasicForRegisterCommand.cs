using FluentValidation;

namespace ESCenter.Mobile.Application.Contracts.Users.Tutors;

public class TutorBasicForRegisterCommand
{
    public Guid Id { get; set; }

    //is tutor related information
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public List<string> Majors { get; set; } = new();
    public List<string> ImageFileUrls { get; set; } = new()
    {
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686459899/z4335058816137_8c84fd04f87afc35b461e273003c7dc3.jpg",
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686723383/Screenshot2023-04-19100710.png"
    };
}

public class TutorBasicForRegisterCommandValidator : AbstractValidator<TutorBasicForRegisterCommand>
{
    public TutorBasicForRegisterCommandValidator()
    {
        // Optional validation for Id (you can adjust based on your needs)
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

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
            .MaximumLength(10)
            .WithMessage("Each major must be between 1 and 10 characters long.");

        // Optional validation for ImageFileUrls list (you can adjust based on your needs)
        RuleForEach(x => x.ImageFileUrls)
            .NotEmpty()
            //.Uri()
            .WithMessage("Please enter a valid image URL.");
    }
}
