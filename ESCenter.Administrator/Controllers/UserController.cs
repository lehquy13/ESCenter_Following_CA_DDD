using ESCenter.Administrator.Utilities;
using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.ServiceImpls.Users.Commands.CreateUpdateUserProfile;
using ESCenter.Admin.Application.ServiceImpls.Users.Commands.DeleteUser;
using ESCenter.Admin.Application.ServiceImpls.Users.Queries.GetLearnerDetail;
using ESCenter.Admin.Application.ServiceImpls.Users.Queries.GetLearners;
using ESCenter.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Route("[controller]")]
public class UserController(ILogger<UserController> logger, ISender sender) : Controller
{
    private void PackStaticListToView()
    {
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
    }

    #region basic user management

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var userDtos = await sender.Send(new GetLearnersQuery());
        if (userDtos.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(userDtos.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        PackStaticListToView();

        var result = await sender.Send(new GetLearnerDetail(id));
        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, [FromForm] LearnerForCreateUpdateDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", userDto);
        }

        try
        {
            var result = await sender.Send(new CreateUpdateUserProfileCommand(userDto));

            if (!result.IsSuccess)
            {
                logger.LogError("Create user failed!");
                return RedirectToAction("Error", "Home");
            }

            PackStaticListToView();

            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                userDto
            );
        }
        catch (Exception ex)
        {
            //Log the error (uncomment ex variable name and write a log.)
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + ex.Message +
                                         "see your system administrator.");
        }

        return View("Edit", userDto);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        PackStaticListToView();
        return View(new LearnerForCreateUpdateDto());
    }


    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LearnerForCreateUpdateDto userForCreateDto)
    {
        var result = await sender.Send(new CreateUpdateUserProfileCommand(userForCreateDto));

        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        PackStaticListToView();

        var result = await sender.Send(new GetLearnerDetail(id));
        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return Helper.RenderRazorViewToString(this, "Delete", result.Value);
    }

    [HttpPost("{id}/delete-confirm")]
    public async Task<IActionResult> DeleteConfirm(Guid id)
    {
        var result = await sender.Send(new DeleteUserCommand(id));

        return result.IsFailure ? RedirectToAction("Error", "Home") : RedirectToAction("Index");
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> Detail(Guid id)
    {
        var result = await sender.Send(new GetLearnerDetail(id));
        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }

    #endregion
}