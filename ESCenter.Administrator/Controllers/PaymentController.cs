using ESCenter.Admin.Application.ServiceImpls.Payments.Commands.CancelPayments;
using ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Get;
using ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Gets;
using ESCenter.Administrator.Utilities;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
[Route("admin/[controller]")]
public class PaymentController(IAppLogger<PaymentController> logger, ISender sender) : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var result = await sender.Send(new GetAllPaymentsQuery());

        return result.IsFailure
            ? RedirectToAction("Error", "Home")
            : View(result.Value);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Detail(Guid id)
    {
        var result = await sender.Send(new GetPaymentDetailQuery(id));

        return result.IsFailure
            ? RedirectToAction("Error", "Home")
            : Helper.RenderRazorViewToString(this, "_Detail", result.Value);
    }

    [HttpGet]
    [Route("{id:guid}/re-open")]
    public async Task<IActionResult> ReOpenPayment(Guid id)
    {
        var result = await sender.Send(new ReOpenPaymentCommand(id));

         return result.IsFailure
             ? Helper.FailResult()
             : Helper.UpdatedThenResetResult();
    }

    [HttpGet]
    [Route("{id:guid}/cancel")]
    public async Task<IActionResult> CancelPayment(Guid id)
    {
        var result = await sender.Send(new CancelPaymentCommand(id));

        return result.IsFailure
            ? Helper.FailResult()
            : Helper.UpdatedThenResetResult();
    }
}