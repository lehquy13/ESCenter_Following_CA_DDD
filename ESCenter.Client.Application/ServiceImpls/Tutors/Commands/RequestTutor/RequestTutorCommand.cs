using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public record RequestTutorCommand(TutorRequestForCreateDto TutorRequestForCreateDto) : ICommandRequest;

public class TutorRequestForCreateDto
{
    public Guid TutorId { get; set; }
    public Guid LearnerId { get; set; }
    public string RequestMessage { get; set; } = null!;
}