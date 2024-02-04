using ESCenter.Application.ServiceImpls.Accounts.Commands.ChangePassword;
using ESCenter.Application.ServiceImpls.Accounts.Commands.Register;
using ESCenter.Application.ServiceImpls.Accounts.Queries.Login;
using MapsterMapper;
using MediatR;
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

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordCommand changePasswordCommand)
    {
        var result = await mediator.Send(changePasswordCommand);

        return Ok();
    }
}