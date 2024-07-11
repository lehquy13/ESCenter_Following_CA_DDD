using ESCenter.Application.Accounts.Commands.ForgetPassword;
using ESCenter.Application.Accounts.Commands.Register;
using ESCenter.Application.Accounts.Queries.Login;
using ESCenter.Application.Accounts.Queries.ValidateToken;
using ESCenter.Application.Contracts.Authentications;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("client/[controller]")]
public class AuthenticationController(ISender mediator, IAppLogger<AuthenticationController> logger)
    : Controller
{
    [Route("")]
    public async Task<IActionResult> Index(string? returnUrl)
    {
        TempData["ReturnUrl"] = returnUrl;
        var validateToken = HttpContext.Session.GetString("access_token");

        if (string.IsNullOrWhiteSpace(validateToken))
        {
            return View("Login", new LoginQuery("", ""));
        }

        var query = new ValidateTokenQuery(validateToken);
        var loginResult = await mediator.Send(query);

        if (loginResult.IsSuccess && HttpContext.Session.GetString("email") != null)
        {
            return RedirectToAction("Index", "Home");
        }

        return await Logout();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterCommand registerRequest)
    {
        var result = await mediator.Send(registerRequest);

        return result.IsSuccess
            ?
            //await StoreCookie(result);
            RedirectToAction("Index", "Home")
            : View(registerRequest);
    }

    [HttpGet]
    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginQuery query)
    {
        var loginResult = await mediator.Send(query);

        if (loginResult.IsSuccess is false)
        {
            ViewBag.isFail = true;
            return View("Login", new LoginQuery("", ""));
        }

        await StoreCookie(loginResult.Value);

        var returnUrl = TempData["ReturnUrl"] as string;
        if (returnUrl is null)
            return RedirectToAction("Index", "Home");

        var value = HttpContext.Session.GetString("Value");

        logger.LogInformation(returnUrl);

        if (value != null)
        {
            HttpContext.Session.Remove("Value");
            await HttpContext.Session.CommitAsync();
            return RedirectToAction("Detail", "Course", new { id = new Guid(value) });
        }

        return Redirect(returnUrl);
    }

    private async Task StoreCookie(AuthenticationResult loginResult)
    {
        HttpContext.Session.SetString("access_token", loginResult.Token);
        HttpContext.Session.SetString("name", loginResult.User.FullName);
        HttpContext.Session.SetString("image", loginResult.User.Avatar);
        await HttpContext.Session.CommitAsync();
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await Task.CompletedTask;
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("forgot-password")]
    public async Task<IActionResult> ForgotPassword()
    {
        await Task.CompletedTask;
        return View();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var loginResult = await mediator.Send(new ForgetPasswordCommand(email));

        return View("SuccessPage", "If you have registered with us, we have sent an email to your registered email.");
    }

    // Confirm email using userId and token
    [HttpGet("confirm-email/{userId}/{token}")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
    {
        var result = await mediator.Send(new ConfirmEmailCommand(userId, token));

        return result.IsSuccess
            ? View("SuccessPage", "Your email has been confirmed successfully. You can now login.")
            : View("Error");
    }
}