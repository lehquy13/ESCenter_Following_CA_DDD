using ESCenter.Admin.Application.ServiceImpls.TutorRequests.Commands.MarkRequestAsDone;
using ESCenter.Admin.Application.ServiceImpls.TutorRequests.Queries.GetAllTutorRequests;
using ESCenter.Domain.Shared.Courses;
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
    public async Task<IActionResult> Index([FromQuery] RequestStatus type)
    {
        var result = await sender.Send(new GetAllTutorRequestsQuery(type));

        return View(result.Value);
    }

    [HttpGet("done/{id:guid}")]
    public async Task<IActionResult> MarkAsDone([FromRoute] Guid id)
    {
        var result = await sender.Send(new MarkRequestAsDoneCommand(id));

        return result.IsFailure
            ? RedirectToAction("Error", "Home")
            : RedirectToAction("Index");
    }
}