namespace ESCenter.Application.Contracts.Courses.Dtos;

public class CourseRequestForDetailDto
{
    public int Id { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public int TutorId { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Verifying";
    public string LearnerName { get; set; } = string.Empty;
    public string LearnerContact { get; set; } = string.Empty;
}