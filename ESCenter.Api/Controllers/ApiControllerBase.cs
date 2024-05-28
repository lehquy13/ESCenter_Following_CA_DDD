using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase(IAppLogger<ApiControllerBase> logger) : ControllerBase
{
    protected readonly IAppLogger<ApiControllerBase> Logger = logger;
}