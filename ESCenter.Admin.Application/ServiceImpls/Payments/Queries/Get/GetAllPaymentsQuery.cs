using ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Gets;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Get;

public record GetPaymentDetailQuery(Guid Guid) : IQueryRequest<PaymentDto>;

