using ESCenter.Application.Accounts.Commands.ChangePassword;
using ESCenter.Application.Accounts.Commands.Register;
using ESCenter.Application.Accounts.Queries.Login;
using ESCenter.Host;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class AuthenticationController(
    IAppLogger<AuthenticationController> logger,
    ISender mediator)
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