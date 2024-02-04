using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Domain.Shared;

public static class EnumProvider
{
    public static List<string> Roles { get; } = Enum.GetNames(typeof(UserRole))
        .AsEnumerable()
        .Where(x => x != "All" && x != "Undefined")
        .ToList();

    public static List<string> Genders { get; } = Enum.GetNames(typeof(GenderEnum))
        .AsEnumerable()
        .Where(x => x != "None")
        .ToList();

    public static List<string> FullGendersOption { get; } = Enum.GetNames(typeof(GenderEnum))
        .AsEnumerable()
        .ToList();

    public static List<string> FullAcademicLevelsOption { get; } = Enum.GetNames(typeof(AcademicLevel))
        .ToList();

    public static List<string> AcademicLevels { get; } = Enum.GetNames(typeof(AcademicLevel))
        .Where(x => x != "Optional")
        .ToList();

    public static List<string> LearningModes { get; } = Enum.GetNames(typeof(LearningMode))
        .ToList();

    public static List<string> Statuses { get; } = Enum.GetNames(typeof(Status))
        .ToList();

    public static List<string> FullRequestStatus { get; } = Enum.GetNames(typeof(RequestStatus))
        .ToList();

    public static T ToEnum<T>(this string value) where T : notnull
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}