using ESCenter.Domain.Aggregates.Tutors.Errors;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class ChangeVerificationRequestDetail : Entity<int>
{
    public string ImageUrl { get; private set; } = null!;

    public int ChangeVerificationRequestId { get; private set; }

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
    public static ChangeVerificationRequestDetail Create(string imageUrl, int changeVerificationRequestId)
    {
        return new ChangeVerificationRequestDetail()
        {
            ImageUrl = imageUrl,
            ChangeVerificationRequestId = changeVerificationRequestId
        };
    }
}