using ESCenter.Domain.Aggregates.Payment;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Commands.CancelPayments;

public class CancelPaymentCommandHandler(
    IRepository<Payment, PaymentId> paymentRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<CancelPaymentCommandHandler> logger
) : CommandHandlerBase<CancelPaymentCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CancelPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetAsync(PaymentId.Create(command.Id), cancellationToken);

        if (payment == null)
        {
            return Result.Fail("Payment not found");
        }

        payment.Cancel();

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail("Fail to cancel payment")
            : Result.Success();
    }
}