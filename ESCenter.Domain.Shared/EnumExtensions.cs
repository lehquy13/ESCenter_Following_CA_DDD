using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Domain.Shared;

public static class EnumProvider
{
    public static List<string> Roles { get; } = Enum.GetNames(typeof(Role))
        .Where(x => x != "All" && x != "Undefined")
        .ToList();

    public static List<string> Genders =>
    [
        "Male",
        "Female",
        "Other"
    ];

    public static List<string> GenderFilters => Enum.GetNames(typeof(Gender))
        .Where(x => x != "Both")
        .ToList();

    public static List<string> FullGendersOption =>
    [
        "Male",
        "Female",
        "Other",
        "Both",
        "None"
    ];

    public static List<string> FullAcademicLevelsOption => Enum.GetNames(typeof(AcademicLevel))
        .ToList();

    public static List<string> AcademicLevels =>
    [
        "UnderGraduated",
        "Graduated",
        "Lecturer",
        "Optional"
    ];

    public static List<string> LearningModes { get; } = Enum.GetNames(typeof(LearningMode))
        .ToList();

    public static List<string> Statuses { get; } = Enum.GetNames(typeof(Status)).ToList();

    public static List<string> StatusWithoutConfirmed { get; } = Enum.GetNames(typeof(Status))
        .Where(x => x != Status.Confirmed.ToString())
        .ToList();

    public static List<string> FullRequestStatus { get; } = Enum.GetNames(typeof(RequestStatus))
        .ToList();

    public static T ToEnum<T>(this string value) where T : notnull
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}