using ESCenter.Administrator.Utilities;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CancelCourseRequest;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CreateCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.DeleteCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.UpdateCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetAllCourses;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseDetail;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseRequest;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetTodayCourses;
using ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutors;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Admin.Application.ServiceImpls.Users.Queries.GetLearners;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
[Route("admin/[controller]")]
public class CourseController(
    ILogger<CourseController> logger,
    IMapper mapper,
    ISender sender)
    : Controller
{
    private async Task PackStaticListToView()
    {
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        ViewData["LearningModes"] = EnumProvider.LearningModes;
        ViewData["Statuses"] = EnumProvider.Statuses;


        var subjects = await sender.Send(new GetAllSubjectsQuery());
        ViewData["Subjects"] = subjects.Value;
    }

    private async Task PackStudentAndTutorList()
    {
        var tutorDtos = await sender.Send(new GetAllTutorsQuery());
        var studentDtos = await sender.Send(new GetLearnersQuery());
        ViewData["TutorDtos"] = tutorDtos;
        ViewData["StudentDtos"] = studentDtos;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index(string? type)
    {
        //var query = new GetObjectQuery<List<ClassInformationDto>>();
        var status = type?.ToEnum<Status>() ?? Status.None;
        var courses = await sender.Send(new GetAllCoursesQuery(status));
        if (courses is { IsSuccess: true, Value: not null })
        {
            return View(courses.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet]
    [Route("today")]
    public async Task<IActionResult> Today()
    {
        //var query = new GetObjectQuery<List<ClassInformationDto>>();
        var courses = await sender.Send(new GetTodayCoursesQuery());
        if (courses is { IsSuccess: true, Value: not null })
        {
            return View("Index", courses.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        await PackStaticListToView();
        await PackStudentAndTutorList();

        var result = await sender.Send(new GetCourseDetailQuery(id));
        ViewBag.Action = "Edit";

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCourse(Guid id, CourseUpdateDto classDto)
    {
        var result = await sender.Send(new UpdateCourseCommand(classDto));

        if (!result.IsSuccess)
        {
            return Helper.FailResult();
        }

        await PackStaticListToView();
        await PackStudentAndTutorList();

        return Helper.UpdatedResult();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await PackStaticListToView();
        //await PackStudentAndTuTorList();

        return View(new CourseForCreateDto());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseForCreateDto classDto)
    {
        var result = await sender.Send(new CreateCourseCommand(classDto));

        if (!result.IsSuccess)
        {
            return View("Create", classDto);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await sender.Send(new GetCourseDetailQuery(id));

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        return
            Helper.RenderRazorViewToString(this, "Delete", result.Value);
    }

    [HttpPost("{id:guid}/delete-confirm")]
    public async Task<IActionResult> DeleteConfirm(Guid id)
    {
        var result = await sender.Send(new DeleteCourseCommand(id));

        if (!result.IsSuccess)
        {
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> Detail(Guid id)
    {
        var result = await sender.Send(new GetCourseDetailQuery(id));

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet]
    [Route("pick-tutor")]
    public async Task<IActionResult> PickTutor()
    {
        var result = await sender.Send(new GetAllTutorsQuery());
        if (result.IsSuccess)
        {
            return Helper.RenderRazorViewToString(this, "_PickTutor", result.Value);
        }

        return BadRequest();
    }

    [HttpGet("view-tutor/{id:guid}")]
    public async Task<IActionResult> ViewTutor(Guid id)
    {
        var result = await sender.Send(new GetTutorDetailQuery(id));

        if (result.IsSuccess)
        {
            return Helper.RenderRazorViewToString(this, "ViewTutor", result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpPost("choose")]
    public IActionResult Choose(Guid tutorId)
    {
        return Json(new { tutorId = tutorId });
    }

    [HttpPost("{courseId}/cancel-request/{requestId}")]
    public async Task<IActionResult> CancelRequest(
        Guid courseId, Guid requestId,
        CourseRequestCancelDto courseRequestCancelDto)
    {
        var result = await sender.Send(new CancelCourseRequestCommand(
            courseRequestCancelDto.CourseRequestId,
            courseRequestCancelDto.CourseId,
            courseRequestCancelDto.Description));

        if (!result.IsSuccess)
        {
            return BadRequest();
        }

        return Helper.UpdatedUsingModalResult();
    }

    [HttpGet("{courseId:guid}/cancel-request/{requestId:guid}")]
    public async Task<IActionResult> OpenCancelRequestDialog(Guid courseId, Guid requestId)
    {
        var result = await sender.Send(new GetCourseRequestQuery(requestId));

        if (!result.IsSuccess)
        {
            return BadRequest();
        }

        return Helper.RenderRazorViewToString(
            this,
            "_EditRequest",
            result.Value
        );
    }
}