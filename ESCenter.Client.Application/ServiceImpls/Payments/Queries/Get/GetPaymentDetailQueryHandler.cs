using ESCenter.Client.Application.ServiceImpls.Payments.Queries.Gets;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Payments.Queries.Get;

public record GetPaymentDetailQuery(Guid Guid) : IQueryRequest<PaymentDto>, IAuthorizationRequest;

public class GetPaymentDetailQueryHandler(
    IRepository<Payment, PaymentId> paymentRepository,
    ICustomerRepository customerRepository,
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
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
            .GetAsync(CustomerId.Create(currentUserService.UserId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail("Tutor not found");
        }

        payment.TutorName = tutor.GetFullName();

        return payment;
    }
}