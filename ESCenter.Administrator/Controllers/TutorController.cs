﻿using ESCenter.Administrator.Utilities;
using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.Contracts.Users.Tutors;
using ESCenter.Admin.Application.ServiceImpls.Customers.Commands.CreateUpdateUserProfile;
using ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Admin.Application.ServiceImpls.TutorRequests.Queries.GetTutorRequestsByTutorId;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.ClearTutorRequests;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.CreateTutor;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateChangeVerificationRequestCommand;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorInformation;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorMajors;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutorsForManagement;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorChangeVerifications;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorMajors;
using ESCenter.Domain.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
[Route("admin/[controller]")]
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
        [FromBody] LearnerForCreateUpdateDto learnerForCreateUpdateDto)
    {
        if (!ModelState.IsValid)
            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                learnerForCreateUpdateDto,
                true
            );
        try
        {
            var result = await sender.Send(new CreateUpdateUserProfileCommand(learnerForCreateUpdateDto));

            if (!result.IsSuccess)
            {
                // TODO: better display error message to user
                return Helper.RenderRazorViewToString(
                    this,
                    "Edit",
                    learnerForCreateUpdateDto,
                    true
                );
            }

            PackStaticListToView();

            // TODO: better display success message to user, the model doesn't fit the view
            return Helper.RenderRazorViewToString(
                this,
                "Edit",
                learnerForCreateUpdateDto
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
            learnerForCreateUpdateDto,
            true
        );
    }

    [HttpPost("{id}/edit-tutor-information")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTutorInformation(
        [FromRoute] Guid id,
        TutorBasicUpdateDto tutorBasicUpdateDto)
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
            var result = await sender.Send(new UpdateTutorInformationCommand(tutorBasicUpdateDto));

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
        ViewData["Majors"] = sender.Send(new GetAllSubjectsQuery()).Result.Value;
        return View(new TutorCreateDto());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TutorProfileCreateDto tutorProfileCreateDto, LearnerForCreateUpdateDto learnerForCreateUpdateDto)
    {
        var tutorCreateDto = new TutorCreateDto
        {
            LearnerForCreateUpdateDto = learnerForCreateUpdateDto,
            TutorProfileCreateDto = tutorProfileCreateDto
        };
        
        if (!ModelState.IsValid)
        {
            var createViewModel = mapper.Map<TutorUpdateDto>(tutorCreateDto);
            return View("Create", new TutorCreateDto());
        }

        var result = await sender.Send(new CreateTutorCommand(tutorCreateDto));

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

    [HttpPost("{tutorId}/edit-tutor-verification-information")]
    public async Task<IActionResult> ModifyChangeVerificationRequest(
        [FromRoute] Guid tutorId,
        [FromForm] bool isApproved)
    {
        var result = await sender.Send(new UpdateChangeVerificationCommand(tutorId, isApproved));

        return !result.IsSuccess ? RedirectToAction("Error", "Home") : RedirectToAction("Index");
    }

    [HttpPost("{tutorId}/edit-tutor-majors")]
    public async Task<IActionResult> EditTutorMajors(
        [FromRoute] Guid tutorId,
        [FromForm] List<int> majorIds)
    {
        var result = await sender.Send(new UpdateTutorMajorsCommand(tutorId, majorIds));

        return !result.IsSuccess ? RedirectToAction("Error", "Home") : Helper.SuccessResult();
    }

    [HttpGet("{id}/view-tutor-request")]
    public async Task<IActionResult> ViewTutorRequests([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetTutorRequestsByTutorIdQuery(id));

        return result.IsSuccess
            ? Helper.RenderRazorViewToString(this, "ViewTutorRequests", result.Value)
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