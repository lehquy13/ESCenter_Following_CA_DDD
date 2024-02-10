using ESCenter.Application.Contracts.Users.Tutors;

namespace ESCenter.Application.Contracts.Users.Learners;

public class TutorRequestForCreateDto
{
    public Guid TutorId { get; set; }
    public Guid LearnerId { get; set; }
    public string RequestMessage { get; set; } = null!;
}

public class TutorRequestForListDto
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public TutorForListDto Tutor { get; set; } = null!;
    
    public Guid LearnerId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; }= string.Empty;
    public string RequestMessage { get; set; } = null!;
}