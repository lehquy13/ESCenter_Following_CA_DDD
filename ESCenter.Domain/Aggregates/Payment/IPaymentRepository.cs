using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Payment;

public interface IPaymentRepository : IRepository
{
    Task<List<Payment>> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken);
    Task<Payment?> GetLatestByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken);
    Task InsertAsync(Payment payment, CancellationToken cancellationToken);
}