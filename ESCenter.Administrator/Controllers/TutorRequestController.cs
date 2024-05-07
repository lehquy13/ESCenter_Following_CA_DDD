using ESCenter.Admin.Application.ServiceImpls.TutorRequests.Commands.MarkRequestAsDone;
using ESCenter.Admin.Application.ServiceImpls.TutorRequests.Queries.GetAllTutorRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
[Route("admin/[controller]")]
public class TutorRequestController(ISender sender)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await sender.Send(new GetAllTutorRequestsQuery());

        return View(result.Value);
    }

    [HttpGet("done")]
    public async Task<IActionResult> MarkAsDone([FromRoute] Guid guid)
    {
        var result = await sender.Send(new MarkRequestAsDoneCommand(guid));

        return result.IsFailure
            ? RedirectToAction("Error", "Home")
            : RedirectToAction("Index");
    }
}