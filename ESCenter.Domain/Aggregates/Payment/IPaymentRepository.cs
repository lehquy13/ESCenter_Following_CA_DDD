using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Payment;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.ConfirmCourse;

public interface IPaymentRepository : IRepository<Payment, PaymentId>
{
    Task<Payment?> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken);
}