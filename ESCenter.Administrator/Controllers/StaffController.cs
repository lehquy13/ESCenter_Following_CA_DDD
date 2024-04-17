using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearnerDetail;
using ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearners;
using ESCenter.Admin.Application.ServiceImpls.Staffs.Commands.CreateStaff;
using ESCenter.Admin.Application.ServiceImpls.Staffs.Commands.DeleteStaff;
using ESCenter.Admin.Application.ServiceImpls.Staffs.Queries.GetStaff;
using ESCenter.Admin.Application.ServiceImpls.Staffs.Queries.GetStaffs;
using ESCenter.Administrator.Utilities;
using ESCenter.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireSuperAdministratorRole")]
[Route("admin/[controller]")]
public class StaffController(ILogger<StaffController> logger, ISender sender) : Controller
{
    private void PackStaticListToView()
    {
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
    }

    #region basic staff management

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var staffDtos = await sender.Send(new GetStaffsQuery());
        if (staffDtos.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(staffDtos.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        PackStaticListToView();

        var result = await sender.Send(new GetStaffQuery(id));
        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, [FromForm] LearnerForCreateUpdateDto staffDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", staffDto);
        }

        try
        {
            var result = await sender.Send(new CreateUpdateStaffProfileCommand(staffDto));

            if (!result.IsSuccess)
            {
                logger.LogError("Create staff failed!");
                return RedirectToAction("Error", "Home");
            }

            PackStaticListToView();

            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                staffDto
            );
        }
        catch (Exception ex)
        {
            //Log the error (uncomment ex variable name and write a log.)
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + ex.Message +
                                         "see your system administrator.");
        }

        return View("Edit", staffDto);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        PackStaticListToView();
        return View(new LearnerForCreateUpdateDto());
    }


    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LearnerForCreateUpdateDto staffForCreateDto)
    {
        var result = await sender.Send(new CreateUpdateStaffProfileCommand(staffForCreateDto));

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
        var result = await sender.Send(new DeleteStaffCommand(id));

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