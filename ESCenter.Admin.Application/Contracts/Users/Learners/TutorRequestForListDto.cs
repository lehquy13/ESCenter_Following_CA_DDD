using ESCenter.Admin.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.Courses;
using Mapster;

namespace ESCenter.Admin.Application.Contracts.Users.Learners;

public class TutorRequestForListDto
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public string TutorFullName { get; set; } = null!;
    public string TutorPhoneNumber { get; set; } = null!;
    public string TutorEmail { get; set; } = null!;

    public Guid LearnerId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RequestMessage { get; set; } = null!;
    public RequestStatus Status { get; set; }
}