using Matt.SharedKernel.Application.Contracts.Primitives;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Gets;

public record GetAllPaymentsQuery() : IQueryRequest<IEnumerable<PaymentDto>>;

public class PaymentDto
{
    public Guid PaymentId { get; init; }
    public Guid TutorId { get; init; }
    public string TutorName { get; set; } = null!;
    public string TutorEmail { get; set; } = null!;
    public Guid CourseId { get; init; }

    public string CourseTitle { get; set; } = null!;

    public string PaymentStatus { get; init; } = null!;
    public string Code { get; init; } = null!;

    
}

public static class PaymentDtoExtensions
{
    public static string Truncate(this string value, int maxChars)
    {
        return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
    }
}