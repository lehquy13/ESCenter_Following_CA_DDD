using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Payments.Commands.MakePayment;

public record MakePaymentCommand(Guid Id) : ICommandRequest, IAuthorizationRequest;