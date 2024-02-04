using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected readonly ILogger<ApiControllerBase> Logger;

    public ApiControllerBase(ILogger<ApiControllerBase> logger)
    {
        Logger = logger;
    }
}