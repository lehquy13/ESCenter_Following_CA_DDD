using ESCenter.Mobile.Application.ServiceImpls.Payments.Commands.MakePayment;
using ESCenter.Mobile.Application.ServiceImpls.Payments.Queries.Gets;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

[Authorize]
public class PaymentController(
    IAppLogger<PaymentController> logger,
    ISender sender)
    : ApiControllerBase(logger)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllPayments()
    {
        var courseResult = await sender.Send(new GetAllPaymentsQuery());

        return Ok(courseResult);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> MakePayment(Guid id)
    {
        var paymentResult = await sender.Send(new MakePaymentCommand(id));

        return Ok(paymentResult);
    }
}