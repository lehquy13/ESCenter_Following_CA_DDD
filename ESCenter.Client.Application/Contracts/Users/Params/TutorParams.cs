using Matt.Paginated;

namespace ESCenter.Client.Application.Contracts.Users.Params;

public class TutorParams : PaginatedParams
{
    public string? SubjectName { get; set; } = string.Empty;
    public string? TutorName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public string? Academic { get; set; } = "Optional";
    public string? Gender { get; set; } = "None";
    public string? Address { get; set; } = string.Empty;
}