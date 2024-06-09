using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Errors;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.DomainServices;

public class PaymentDomainService(
    ICourseRepository courseRepository,
    IRepository<Payment, PaymentId> paymentRepository
) : IPaymentDomainService
{
    public async Task<Result> CreatePayment(CourseId courseId)
    {
        var course = await courseRepository.GetAsync(courseId);

        if (course == null)
        {
            return Result.Fail(DomainServiceErrors.CourseNotFound);
        }

        if (course.Status != Status.OnProgressing ||
            course.TutorId == null)
        {
            return Result.Fail(DomainServiceErrors.InvalidCourseStatusForPayment);
        }

        var payment = Payment.Create(course.TutorId, course.Id, course.ChargeFee.Amount);

        await paymentRepository.InsertAsync(payment);

        return Result.Success();
    }

    public Task<Result> CancelPayment()
    {
        throw new NotImplementedException();
    }
}