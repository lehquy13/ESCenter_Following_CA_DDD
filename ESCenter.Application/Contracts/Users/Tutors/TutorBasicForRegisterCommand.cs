namespace ESCenter.Application.Contracts.Users.Tutors;

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