using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class PaymentRepository(AppDbContext appDbContext) : IPaymentRepository
{
    public async Task<Payment?> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken)
    {
        return await appDbContext.Payments
            .FirstOrDefaultAsync(x => x.CourseId == courseId, cancellationToken);
    }

    public Task InsertAsync(Payment payment, CancellationToken cancellationToken)
    {
        appDbContext.Payments.Add(payment);
        return Task.CompletedTask;
    }
}