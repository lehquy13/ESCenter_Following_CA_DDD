using ESCenter.Domain.Aggregates.TutorRequests.DomainEvents;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.TutorRequests;

public class TutorRequest : AuditedAggregateRoot<TutorRequestId>
{
    public TutorId TutorId { get; private set; } = null!;
    public CustomerId LearnerId { get; private set; } = null!;
    public string Message { get; private set; } = string.Empty;
    public RequestStatus RequestStatus { get; private set; } = RequestStatus.Pending;

    private TutorRequest()
    {
    }

    public static TutorRequest Create(
        TutorId tutorId,
        CustomerId learnerId,
        string message)
    {
        var tutorRequest = new TutorRequest()
        {
            Id = TutorRequestId.Create(),
            TutorId = tutorId,
            LearnerId = learnerId,
            Message = message
        };
        
        tutorRequest.DomainEvents.Add(new TutorRequestCreatedDomainEvent(tutorRequest));

        return tutorRequest;
    }
    
    public void Approve()
    {
        RequestStatus = RequestStatus.Approved;
    }
}