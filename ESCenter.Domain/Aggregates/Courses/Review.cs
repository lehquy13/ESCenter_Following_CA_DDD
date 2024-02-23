using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses;

public class Review : Entity<ReviewId>
{
    public short Rate { get; private set; }
    public string Detail { get; private set; }
    public CourseId CourseId { get; private set; }

    private Review(short rate, string detail, CourseId courseId)
    {
        Rate = rate;
        Detail = detail;
        CourseId = courseId;
    }

    public static Review Create(short rate, string detail, CourseId courseId)
    {
        return new Review(rate, detail, courseId);
    }
}