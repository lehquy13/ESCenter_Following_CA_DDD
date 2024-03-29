using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESCenter.Host;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase(IAppLogger<ApiControllerBase> logger) : ControllerBase
{
    protected readonly IAppLogger<ApiControllerBase> Logger = logger;
}