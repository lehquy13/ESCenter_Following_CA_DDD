using ESCenter.Client.Application.Contracts.Courses.Dtos;

namespace ESCenter.Client.Models;

public class CourseDetailViewModel
{
    public CourseDetailDto CourseDetailDto { get; set; } = new();
    public IEnumerable<CourseForListDto> RelatedCourses { get; set; } = null!;

}