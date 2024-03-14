using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("[controller]")]
[Authorize]
public class SubjectController : Controller
{
    private readonly ILogger<SubjectController> _logger;
    //dependencies 
    private readonly ISender _mediator;
    private readonly IMapper _mapper;



    public SubjectController(ILogger<SubjectController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _mediator = sender;
        _mapper = mapper;
    }
    
    [HttpGet("Subjects")]
    public async Task<IActionResult> Subjects(string? tutorGuid)
    {
        var query = new GetObjectQuery<PaginatedList<SubjectDto>>();
        
        if (tutorGuid != null)
        {
            query.ObjectId = new Guid(tutorGuid);
        }
        var subjectDtos = await _mediator.Send(query);
        return Helper.RenderRazorViewToString(this, "_Subjects", subjectDtos.Value);
    }
   
    [HttpGet("Detail")]
    public async Task<IActionResult> Detail(Guid? id)
    {
        if (id == null || id.Equals(Guid.Empty))
        {
            return NotFound();
        }

        var query = new GetObjectQuery<SubjectDto>() { ObjectId = (Guid)id };

        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return View(result.Value);

        }
        return RedirectToAction("Error", "Home");
    }


}

