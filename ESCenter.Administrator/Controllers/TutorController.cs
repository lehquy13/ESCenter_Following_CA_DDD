using ESCenter.Administrator.Utilities;
using ESCenter.Application.Contracts.Users.BasicUsers;
using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Accounts.Commands.CreateUpdateLearnerProfile;
using ESCenter.Application.ServiceImpls.Accounts.Commands.UpdateTutorProfile;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.ClearTutorRequests;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.CreateTutor;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.UpdateChangeVerificationRequestCommand;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorChangeVerifications;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorDetail;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorMajors;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorRequests;
using ESCenter.Application.ServiceImpls.Clients.Tutors.Queries;
using ESCenter.Domain.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GetAllTutorsQuery =
    ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutorsForManagement.GetAllTutorsQuery;

namespace ESCenter.Administrator.Controllers;

[Route("[controller]")]
public class TutorController(ILogger<TutorController> logger, IMapper mapper, ISender sender)
    : Controller
{
    private void PackStaticListToView()
    {
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
    }

    #region basic Tutor management

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var tutorsResult = await sender.Send(new GetAllTutorsQuery());
        if (tutorsResult is { IsSuccess: true, Value: not null })
        {
            return View(tutorsResult.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        PackStaticListToView();

        var result = await sender.Send(new GetTutorDetailQuery(id));
        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpPost("{id}/edit-profile")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(
        [FromRoute] Guid id,
        [FromBody] LearnerForCreateUpdateDto tutorForProfileDto)
    {
        if (!ModelState.IsValid)
            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                tutorForProfileDto,
                true
            );
        try
        {
            var result = await sender.Send(new CreateUpdateLearnerProfileCommand(tutorForProfileDto));

            if (!result.IsSuccess)
            {
                // TODO: better display error message to user
                return Helper.RenderRazorViewToString(
                    this,
                    "Edit",
                    tutorForProfileDto,
                    true
                );
            }

            PackStaticListToView();

            // TODO: better display success message to user, the model doesn't fit the view
            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                tutorForProfileDto
            );
        }
        catch (Exception ex)
        {
            //Log the error (uncomment ex variable name and write a log.)
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + ex.Message +
                                         "see your system administrator.");
        }

        // TODO: better display success message to user, the model doesn't fit the view
        return Helper.RenderRazorViewToString(
            this,
            "Edit",
            tutorForProfileDto,
            true
        );
    }

    [HttpPost("{id}/edit-tutor-information")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTutorInformation(
        [FromRoute] Guid id,
        TutorBasicForUpdateDto tutorBasicForUpdateDto)
    {
        if (!ModelState.IsValid)
            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                "",
                true
            );
        try
        {
            var result = await sender.Send(new UpdateTutorProfileCommand(tutorBasicForUpdateDto));

            if (!result.IsSuccess)
            {
                return Helper.RenderRazorViewToString(
                    this,
                    "Edit",
                    "",
                    true
                );
            }

            PackStaticListToView();

            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                ""
            );
        }
        catch (Exception ex)
        {
            //Log the error (uncomment ex variable name and write a log.)
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + ex.Message +
                                         "see your system administrator.");
        }

        return Helper.RenderRazorViewToString(
            this,
            "Edit",
            "",
            true
        );
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        PackStaticListToView();
        return View(new TutorForDetailDto());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TutorCreateUpdateDto tutorCreateUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            var createViewModel = mapper.Map<TutorForDetailDto>(tutorCreateUpdateDto);
            return View("Create", createViewModel);
        }

        var result = await sender.Send(new CreateTutorCommand(tutorCreateUpdateDto));

        if (!result.IsSuccess)
        {
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> Detail(Guid id)
    {
        var result = await sender.Send(new GetTutorDetailQuery(id));

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    #endregion

    [HttpGet("subjects/{id}")]
    public async Task<IActionResult> Subjects(Guid id)
    {
        var subjectDtos = await sender.Send(new GetTutorMajorsQuery(id));
        return Helper
            .RenderRazorViewToString(this, "_Subjects", subjectDtos.Value);
    }

    [HttpGet("{id}/edit-tutor-verification-information")]
    public async Task<IActionResult> EditVerification(Guid id)
    {
        var result = await sender.Send(new GetTutorChangeVerificationsQuery(id));

        if (!result.IsSuccess)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }

    [HttpPost("{tutorId}/edit-tutor-verification-information/{changeVerificationRequestId}")]
    public async Task<IActionResult> EditTutorVerificationInformation(
        [FromRoute] Guid tutorId,
        [FromRoute] int changeVerificationRequestId)
    {
        var result = await sender.Send(new UpdateChangeVerificationCommand(tutorId, changeVerificationRequestId, true));

        return !result.IsSuccess ? RedirectToAction("Error", "Home") : RedirectToAction("Index");
    }

    [HttpGet("{id}/view-tutor-request")]
    public async Task<IActionResult> ViewTutorRequests([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetTutorRequestQuery(id));

        return result.IsSuccess
            ? View(result.Value)
            : RedirectToAction("Error", "Home");
    }

    [HttpGet("{id}/clear-tutor-request")]
    public async Task<IActionResult> Done([FromRoute] Guid id)
    {
        var result = await sender.Send(new ClearTutorRequestsCommand(id));

        return result.IsSuccess
            ? RedirectToAction("ViewTutorRequests", new { id })
            : RedirectToAction("Error", "Home");
    }
}