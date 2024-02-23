using ESCenter.Domain.Aggregates.Tutors.Errors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class ChangeVerificationRequestDetail : Entity<ChangeVerificationRequestDetailId>
{
    public string ImageUrl { get; private set; } = null!;

    public ChangeVerificationRequestId ChangeVerificationRequestId { get; private set; } = null!;

    private ChangeVerificationRequestDetail()
    {
    }

    public static ChangeVerificationRequestDetail Create(string imageUrl)
    {
        if (imageUrl.Length < 1)
        {
            throw new ArgumentException(TutorError.ImageUrlCannotBeEmpty);
        }
        return new ChangeVerificationRequestDetail()
        {
            ImageUrl = imageUrl
        };
    }
    
    // TODO: Consider to remove
    public static ChangeVerificationRequestDetail Create(string imageUrl, ChangeVerificationRequestId changeVerificationRequestId)
    {
        return new ChangeVerificationRequestDetail()
        {
            ImageUrl = imageUrl,
            ChangeVerificationRequestId = changeVerificationRequestId
        };
    }
}