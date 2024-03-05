using ESCenter.Domain.Shared.Courses;
using Matt.Paginated;

namespace ESCenter.Admin.Application.Contracts.Courses.Params;

public class CourseParams : PaginatedParams
{
    public string SubjectName { get; set; } = string.Empty;
    public Status? Status { get; set; }
  
    public string Filter = string.Empty;
}