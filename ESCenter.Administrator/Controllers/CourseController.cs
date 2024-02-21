using ESCenter.Administrator.Utilities;
using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Application.Contracts.Courses.Params;
using ESCenter.Application.ServiceImpls.Admins.Courses.Commands.CancelCourseRequest;
using ESCenter.Application.ServiceImpls.Admins.Courses.Commands.CreateCourse;
using ESCenter.Application.ServiceImpls.Admins.Courses.Commands.DeleteCourse;
using ESCenter.Application.ServiceImpls.Admins.Courses.Commands.UpdateCourse;
using ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetAllCourses;
using ESCenter.Application.ServiceImpls.Admins.Courses.Queries.GetCourseDetail;
using ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubjects;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutors;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorDetail;
using ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearners;
using ESCenter.Application.ServiceImpls.Clients.Courses.Queries.GetCourses;
using ESCenter.Domain.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Route("[controller]")]
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


        var subjects = await sender.Send(new GetSubjectsQuery());
        ViewData["Subjects"] = subjects.Value;
    }

    private async Task PackStudentAndTuTorList()
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
        var query = new CourseParams()
        {
            Filter = type ?? ""
        };

        var courses = await sender.Send(new GetAllCoursesQuery());
        if (courses is { IsSuccess: true, Value: not null })
        {
            return View(courses.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        await PackStaticListToView();
        await PackStudentAndTuTorList();

        var result = await sender.Send(new GetCourseDetailQuery(id));
        ViewBag.Action = "Edit";

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCourse(CourseForDetailDto classDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", classDto);
        }

        // TODO: Check does it map correctly
        var courseUpdateDto = mapper.Map<CourseUpdateDto>(classDto);

        var result = await sender.Send(new UpdateCourseCommand(courseUpdateDto));

        if (!result.IsSuccess)
        {
            return View("Edit", classDto);
        }

        await PackStaticListToView();
        await PackStudentAndTuTorList();

        return Helper.RenderRazorViewToString(this, "Edit", classDto);
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

    [HttpGet("{id}/delete")]
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

    [HttpPost("{id}/delete-confirm")]
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
            return Helper.RenderRazorViewToString(this, "PickTutor", result.Value);
        }

        return BadRequest();
    }

    [HttpGet("view-tutor/{id}")]
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
    public async Task<IActionResult> CancelRequest(Guid courseId, Guid requestId)
    {
        var result = await sender.Send(new CancelCourseRequestCommand(requestId));

        if (!result.IsSuccess)
        {
            return BadRequest();
        }

        return Json(new
        {
            res = true
        });
    }
}