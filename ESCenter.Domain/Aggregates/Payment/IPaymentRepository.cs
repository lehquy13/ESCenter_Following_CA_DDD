using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Payment;

public interface IPaymentRepository : IRepository
{
    Task<Payment?> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken);
}