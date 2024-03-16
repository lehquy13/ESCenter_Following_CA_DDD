using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("[controller]")]
[Authorize]
public class SubjectController(ISender sender) : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(int id)
    {
        var query = new GetSubjectQuery(id);
        var result = await sender.Send(query);
        
        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }
}