using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class PaymentRepository(AppDbContext appDbContext) : IPaymentRepository
{
    public async Task<List<Payment>> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken)
    {
        return await appDbContext.Payments
            .Where(x => x.CourseId == courseId).ToListAsync(cancellationToken);
    }
    
    public async Task<Payment?> GetLatestByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken)
    {
        return await appDbContext.Payments
            .Where(x => x.CourseId == courseId)
            .OrderByDescending(x => x.CreationTime)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public Task InsertAsync(Payment payment, CancellationToken cancellationToken)
    {
        appDbContext.Payments.Add(payment);
        return Task.CompletedTask;
    }
}