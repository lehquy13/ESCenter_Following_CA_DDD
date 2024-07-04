using ESCenter.Domain.Aggregates.Courses.Errors;
using Matt.Auditing;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class Review : ValueObject, IAuditedObject
{
    private const short MinRate = 1;
    private const short MaxRate = 5;
    private const int MaxDetailLength = 500;

    public short Rate { get; private set; }
    public string Detail { get; private set; } = null!;


    public DateTime CreationTime { get; private set; }
    public string? CreatorId { get; private set; }
    public DateTime? LastModificationTime { get; private set; }
    public string? LastModifierId { get; private set; }

    private Review()
    {
    }

    public static Result<Review> Create(short rate, string detail, CourseId courseId)
    {
        if (rate is < MinRate or > MaxRate)
        {
            return Result.Fail(CourseDomainError.InvalidReviewRate);
        }

        if (detail.Length > MaxDetailLength)
        {
            return Result.Fail(CourseDomainError.InvalidDetailLength);
        }

        return new Review()
        {
            Rate = rate,
            Detail = detail,
            CreationTime = DateTime.UtcNow,
            LastModificationTime = DateTime.UtcNow
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Rate;
        yield return Detail;
    }
}