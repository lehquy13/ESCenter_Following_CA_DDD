using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Commands.CancelPayments;

public record CancelPaymentCommand(Guid Id) : ICommandRequest;
public record ReOpenPaymentCommand(Guid Id) : ICommandRequest;