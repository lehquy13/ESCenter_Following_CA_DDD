using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Payments.Queries.Gets;

public class PaymentDto
{
    public Guid PaymentId { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string TutorName { get; set; } = null!;
}

public record GetAllPaymentsQuery() : IQueryRequest<IEnumerable<PaymentDto>>;