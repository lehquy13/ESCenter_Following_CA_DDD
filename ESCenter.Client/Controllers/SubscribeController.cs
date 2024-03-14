using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("[controller]")]
public class SubscribeController : Controller
{
    private readonly IAppLogger<SubscribeController> _logger;
    //dependencies 
    private readonly ISender _mediator;
    private readonly IMapper _mapper;



    public SubscribeController(IAppLogger<SubscribeController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _mediator = sender;
        _mapper = mapper;
    }
    
    [HttpGet("Subscribe")]
    public async Task<IActionResult> Subscribe(string mail)
    {
        var query = new EmailSubscriptionCommand(mail);
        
        var result = await _mediator.Send(query);
        if (result)
        {
            return RedirectToAction("SuccessPage", "Home");
        }
        return RedirectToAction("FailPage", "Home");
    }
    [HttpGet("UnSubscribe")]
    public async Task<IActionResult> UnSubscribe(string mail)
    {
        var query = new EmailUnSubscriptionCommand(mail);
        
        var result = await _mediator.Send(query);
        if (result)
        {
            return RedirectToAction("SuccessPage", "Home");
        }
        return RedirectToAction("FailPage", "Home");
    }
   
  


}

