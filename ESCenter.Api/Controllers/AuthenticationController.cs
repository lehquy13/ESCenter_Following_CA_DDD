using ESCenter.Host;
using ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.ChangePassword;
using ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.Register;
using ESCenter.Mobile.Application.ServiceImpls.Accounts.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class AuthenticationController(
    ILogger<AuthenticationController> logger,
    IMediator mediator)
    : ApiControllerBase(logger)
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("change-password/{id:guid}")]
    public async Task<IActionResult> ChangePassword(Guid id,
        [FromBody] ChangePasswordCommand changePasswordCommand)
    {
        var result = await mediator.Send(changePasswordCommand);

        return Ok(result);
    }
}