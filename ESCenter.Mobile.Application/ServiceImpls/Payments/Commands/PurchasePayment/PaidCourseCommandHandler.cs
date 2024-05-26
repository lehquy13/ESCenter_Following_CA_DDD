using ESCenter.Domain.Aggregates.Payment;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Mobile.Application.ServiceImpls.Payments.Commands.PurchasePayment;

public class PaidCourseCommandHandler(
    IRepository<Payment, PaymentId> paymentRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger
) : CommandHandlerBase<PaidCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(PaidCourseCommand command, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetAsync(PaymentId.Create(command.Id), cancellationToken);

        if (payment is null)
        {
            return Result.Fail("Payment not found");
        }

        payment.SetTutorPaid();

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}