using ESCenter.Client.Application.ServiceImpls.Subscriber;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("client/[controller]")]
public class SubscribeController(ISender sender)
    : Controller
{
    [HttpGet("subscribe")]
    public async Task<IActionResult> Subscribe(string mail)
    {
        var query = new SubscribeCommand(mail);
        var result = await sender.Send(query);

        if (result.IsSuccess)
        {
            return RedirectToAction("SuccessPage", "Home");
        }

        return RedirectToAction("FailPage", "Home");
    }

    [HttpGet("unsubscribe")]
    public async Task<IActionResult> UnSubscribe(string mail)
    {
        var query = new UnSubscribeCommand(mail);
        var result = await sender.Send(query);

        if (result.IsFailure)
        {
            return RedirectToAction("SuccessPage", "Home");
        }

        return RedirectToAction("FailPage", "Home");
    }
}