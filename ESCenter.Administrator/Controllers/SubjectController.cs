using ESCenter.Administrator.Utilities;
using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Commands.DeleteSubject;
using ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Commands.UpsertSubject;
using ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Queries.GetSubject;
using ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Queries.GetSubjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Route("[controller]")]
public class SubjectController(ILogger<SubjectController> logger, ISender sender) : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var result = await sender.Send(new GetSubjectAllsQuery());

        if (result.IsFailure || result.Value is null)
        {
            logger.LogError("GetSubjectsQuery failed: {Message}", result.DisplayMessage);
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await sender.Send(new GetSubjectQuery(id));

        if (result.IsFailure || result.Value is null)
        {
            logger.LogError("Get subject {Id} failed: {Message}", id, result.DisplayMessage);
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, SubjectDto subjectDto)
    {
        if (!ModelState.IsValid)
        {
            return View(subjectDto);
        }

        var result = await sender.Send(new UpsertSubjectCommand(subjectDto));

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + result.DisplayMessage +
                                         "see your system administrator.");
            return View(subjectDto);
        }

        return Helper.RenderRazorViewToString(this, "Edit", subjectDto);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new SubjectDto());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubjectDto subjectDto)
    {
        if (!ModelState.IsValid)
        {
            return View(subjectDto);
        }

        var result = await sender.Send(new UpsertSubjectCommand(subjectDto));

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + result.DisplayMessage +
                                         "see your system administrator.");
            return View(subjectDto);
        }

        return Helper.RenderRazorViewToString(this, "Create", subjectDto);
    }

    [HttpGet("{id}/delete-confirm")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteSubjectCommand(id));

        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> Detail(int id)
    {
        var result = await sender.Send(new GetSubjectQuery(id));

        if (result.IsFailure)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(result.Value);
    }
}