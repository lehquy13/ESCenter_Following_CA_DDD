using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CancelCourseRequest;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.ConfirmCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CreateCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.DeleteCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.SetCourseTutor;
using ESCenter.Admin.Application.ServiceImpls.Courses.Commands.UpdateCourse;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetAllCourses;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseDetail;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseRequest;
using ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetTodayCourses;
using ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearners;
using ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutors;
using ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Administrator.Utilities;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
[Route("admin/[controller]")]
public class CourseController(ISender sender) : Controller
{
    private async Task PackStaticListToView()
    {
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.GenderFilters;
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
        var courses = await sender.Send(new GetTodayCoursesQuery());

        if (courses is { IsSuccess: true })
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

        if (result.IsSuccess)
        {
            var tutors = await sender.Send(new GetAllTutorsBySubjectIdQuery(result.Value.SubjectId));

            ViewData["Tutors"] = tutors.Value;

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

        return Helper.UpdatedResult();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await PackStaticListToView();

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

        return !result.IsSuccess ? RedirectToAction("Error", "Home") : RedirectToAction("Index");
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
    [Route($"pick-tutor")]
    public async Task<IActionResult> PickTutor([FromQuery] int subjectId)
    {
        var result = await sender.Send(new GetAllTutorsBySubjectIdQuery(subjectId));

        return result.IsSuccess
            ? Helper.RenderRazorViewToString(this, "_PickTutor", result.Value)
            : Helper.FailResult();
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

    [HttpPost("{courseId:guid}/cancel-request/{requestId:guid}")]
    public async Task<IActionResult> CancelRequest(
        Guid courseId, Guid requestId,
        CourseRequestCancelDto courseRequestCancelDto)
    {
        var result = await sender.Send(new CancelCourseRequestCommand(
            courseRequestCancelDto.CourseRequestId,
            courseRequestCancelDto.CourseId,
            courseRequestCancelDto.Description));

        return !result.IsSuccess ? Helper.FailResult() : Helper.UpdatedThenResetResult();
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

    [HttpPost("{courseId:guid}/set-tutor")]
    public async Task<IActionResult> SetTutor([FromRoute] Guid courseId, [FromForm] Guid tutorId)
    {
        var result = await sender.Send(new SetCourseTutorCommand(courseId, tutorId));

        return result.IsFailure ? Helper.FailResult() : Helper.UpdatedThenResetResult();
    }

    [HttpGet("{courseId:guid}/confirm-course")]
    public async Task<IActionResult> ConfirmCourse(Guid courseId)
    {
        var result = await sender.Send(new ConfirmCourseCommand(courseId));

        return result.IsFailure ? Helper.FailResult(result.DisplayMessage) : Helper.SuccessResult();
    }

    [HttpGet("{courseId:guid}/cancel-course-with-refund")]
    public async Task<IActionResult> CancelWithRefund(CancelCourseWithRefundCommand command)
    {
        var result = await sender.Send(command);

        return result.IsFailure ? Helper.FailResult() : Helper.UpdatedThenResetResult();
    }
}