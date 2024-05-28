using ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Gets;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Get;

public class GetPaymentDetailQueryHandler(
    IRepository<Payment, PaymentId> paymentRepository,
    ICustomerRepository customerRepository,
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetPaymentDetailQueryHandler> logger,
    IMapper mapper
) : QueryHandlerBase<GetPaymentDetailQuery, PaymentDto>(logger, mapper)
{
    public override async Task<Result<PaymentDto>> Handle(GetPaymentDetailQuery request,
        CancellationToken cancellationToken)
    {
        var paymentQueryable = paymentRepository.GetAll()
            .Where(x => x.Id == PaymentId.Create(request.Guid))
            .Join(courseRepository.GetAll(),
                payment => payment.CourseId,
                course => course.Id,
                (payment, course) => new PaymentDto()
                {
                    PaymentId = payment.Id.Value,
                    TutorId = payment.TutorId.Value,
                    CourseId = payment.CourseId.Value,
                    CourseTitle = course.Title,
                    PaymentStatus = payment.PaymentStatus.ToString(),
                    Code = payment.Code
                });

        var payment = await asyncQueryableExecutor
            .FirstOrDefaultAsync(paymentQueryable, false, cancellationToken);

        if (payment is null)
        {
            return Result.Fail("Payment not found");
        }

        var tutor = await customerRepository
            .GetTutorByTutorId(TutorId.Create(payment.TutorId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail("Tutor not found");
        }

        payment.TutorName = tutor.GetFullName();
        payment.TutorEmail = tutor.Email;

        return payment;
    }
}