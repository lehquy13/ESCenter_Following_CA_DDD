using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Payments.Commands.PurchasePayment;

public record PaidCourseCommand(Guid Id) : ICommandRequest, IAuthorizationRequest;