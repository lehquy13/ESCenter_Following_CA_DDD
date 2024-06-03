using ESCenter.Client.Application.ServiceImpls.Payments.Queries.Gets;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Payments.Queries.Get;

public record GetPaymentDetailQuery(Guid Guid) : IQueryRequest<PaymentDto>, IAuthorizationRequest;

