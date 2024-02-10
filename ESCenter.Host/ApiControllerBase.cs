using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESCenter.Host;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase(ILogger<ApiControllerBase> logger) : ControllerBase
{
    protected readonly ILogger<ApiControllerBase> Logger = logger;
}