using ESCenter.Application.Contracts.Commons.Primitives;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class SubjectDto : EntityDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}