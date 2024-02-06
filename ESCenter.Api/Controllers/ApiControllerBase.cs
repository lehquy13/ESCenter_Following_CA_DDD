using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase(ILogger<ApiControllerBase> logger) : ControllerBase
{
    protected readonly ILogger<ApiControllerBase> Logger = logger;
}