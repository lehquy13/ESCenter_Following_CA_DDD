using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISender _sender;

    
    public HomeController(ILogger<HomeController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var query = new PopularTutorsQuery();
        var result = await _sender.Send(query);
        return View(result);
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
        return View(  "SuccessPage",
        
            message?? "We have received your request."
        
        );
    } 
    public IActionResult FailPage()
    {
        return View();
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