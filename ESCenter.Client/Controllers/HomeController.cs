using System.Diagnostics;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.PopularTutors;
using ESCenter.Client.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

public class HomeController(ISender sender) : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var query = new PopularTutorsQuery();
        var result = await sender.Send(query);

        return View(result.Value);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult SuccessPage(string? message)
    {
        return View("SuccessPage",
            message ?? "We have received your request."
        );
    }

    public IActionResult FailPage(string? message)
    {
        ViewBag.RequestedMessage = message!;
        return View("FailPage");
    }

    public IActionResult SuccessRequestPage()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}