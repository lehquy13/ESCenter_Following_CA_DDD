using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Gets;

public record GetAllPaymentsQuery() : IQueryRequest<IEnumerable<PaymentDto>>;

public class PaymentDto
{
    public Guid PaymentId { get; set; }
    public Guid TutorId { get; set; }
    public string TutorName { get; set; } = null!;
    public string TutorEmail { get; set; } = null!;
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public string Code { get; set; } = null!;
}