using ESCenter.Domain.Aggregates.Tutors.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class ChangeVerificationRequest : Entity<int>
{
    private List<ChangeVerificationRequestDetail> _changeVerificationRequestDetails = new();
    public IdentityGuid TutorId { get; private set; } = null!;

    public RequestStatus RequestStatus { get; private set; }

    public IReadOnlyCollection<ChangeVerificationRequestDetail> ChangeVerificationRequestDetails
        => _changeVerificationRequestDetails.AsReadOnly();

    private ChangeVerificationRequest()
    {
    }
    
    public static ChangeVerificationRequest Create(IdentityGuid tutorId, List<string> urls)
    {
        if (urls.Count < 1)
        {
            throw new ArgumentException(TutorError.InvalidImageUrls);
        }

        return new ChangeVerificationRequest()
        {
            TutorId = tutorId,
            RequestStatus = RequestStatus.Pending,
            _changeVerificationRequestDetails =
                urls.Select(url => ChangeVerificationRequestDetail.Create(url)).ToList()
        };
    }

    public void Approve()
    {
        RequestStatus = RequestStatus.Success;
    }
    
    public void Reject()
    {
        RequestStatus = RequestStatus.Canceled;
    }
}