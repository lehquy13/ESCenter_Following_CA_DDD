using ESCenter.Domain.Aggregates.Courses.Errors;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.Entities;

public class Review : Entity<ReviewId>
{
    private const short MinRate = 1;
    private const short MaxRate = 5;
    private const int MaxDetailLength = 500;
    
    public short Rate { get; private set; }
    public string Detail { get; private set; } = null!;
    public CourseId CourseId { get; private set; } = null!;

    private Review()
    {
    }

    public static Result<Review> Create(short rate, string detail, CourseId courseId)
    {
        if (rate < MinRate || rate > MaxRate)
        {
            return Result.Fail(CourseDomainError.InvalidReviewRate);
        }
        
        if (detail.Length > MaxDetailLength)
        {
            return Result.Fail(CourseDomainError.InvalidDetailLength);
        }
        
        return new Review()
        {
            Id = ReviewId.Create(),
            Rate = rate,
            Detail = detail,
            CourseId = courseId
        };
    }
}