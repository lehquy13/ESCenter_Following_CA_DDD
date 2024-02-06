using ESCenter.Application.Contract.Users.Tutors;

namespace ESCenter.Application.Contract.Users.Learners;

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
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Name { get; set; }= string.Empty;
    public string RequestMessage { get; set; } = null!;
}