using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.Entities;

public class Review : Entity<ReviewId>
{
    public short Rate { get; private set; }
    public string Detail { get; private set; } = null!;
    public CourseId CourseId { get; private set; } = null!;

    private Review()
    {
    }

    public static Review Create(short rate, string detail, CourseId courseId)
    {
        return new Review()
        {
            Rate = rate,
            Detail = detail,
            CourseId = courseId
        };
    }
}