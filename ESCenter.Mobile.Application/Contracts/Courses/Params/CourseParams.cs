using Matt.Paginated;

namespace ESCenter.Mobile.Application.Contracts.Courses.Params;

public class CourseParams : PaginatedParams
{
    public string SubjectName { get; set; } = string.Empty;
}