using ESCenter.Application.ServiceImpls.Accounts.Commands.ForgetPassword;
using ESCenter.Application.ServiceImpls.Accounts.Queries.Login;
using ESCenter.Application.ServiceImpls.Accounts.Queries.ValidateToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ESCenter.Administrator.Controllers;

[Route("[controller]")]
[Route("")]
public class AuthenticationController(ILogger<AuthenticationController> logger, ISender sender)
    : Controller
{
    private readonly ILogger<AuthenticationController> _logger = logger;

    [Route("")]
    public async Task<IActionResult> Index(string? returnUrl)
    {
        TempData["ReturnUrl"] = returnUrl;
        string? validateToken = Request.Headers[HeaderNames.Authorization];

        if (validateToken is null)
        {
            return View("Login", new LoginQuery("", ""));
        }

        var query = new ValidateTokenQuery(validateToken);

        var validateResult = await sender.Send(query);

        if (validateResult.IsFailure)
        {
            return View("Login", new LoginQuery("", ""));
        }

        return RedirectToAction("Index", "Home");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginQuery request)
    {
        var loginResult = await sender.Send(request);

        if (loginResult.IsFailure || loginResult.Value is null)
        {
            logger.LogError("Login failed: {Message}", loginResult.DisplayMessage);
            
            ViewBag.isFail = true;
            return View("Login", new LoginQuery("", ""));
        }

        //store token into session
        HttpContext.Session.SetString("access_token", loginResult.Value.Token);
        HttpContext.Session.SetString("name", loginResult.Value.User.FullName);
        HttpContext.Session.SetString("image", loginResult.Value.User.Image);

        if (TempData["ReturnUrl"] is not string returnUrl)
        {
            return RedirectToAction("Index", "Home");
        }

        return Redirect(returnUrl);
    }


    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return View("Login", new LoginQuery("", ""));
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var forgotPasswordResult = await sender.Send(new ForgetPasswordCommand(email));

        if (forgotPasswordResult.IsFailure)
        {
            logger.LogError("Forgot password failed: {Message}", forgotPasswordResult.DisplayMessage);
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index", "Home");
    }
}