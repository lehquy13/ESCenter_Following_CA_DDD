using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.Contracts.Courses.Params;
using ESCenter.Client.Application.ServiceImpls.Courses.Commands.CreateCourse;
using ESCenter.Client.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourseDetail;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourses;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetRelatedCourses;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Models;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("client/[controller]")]
public class CourseController(ISender mediator) : Controller
{
    private const int PageSize = 10;

    private async Task PackStaticListToView()
    {
        var subjectResult = await mediator.Send(new GetSubjectsQuery());
        ViewData["Subjects"] = subjectResult.Value;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index(int pageIndex = 1, string subjectName = "")
    {
        var courseParams = new CourseParams()
        {
            PageSize = PageSize,
            PageIndex = pageIndex,
            SubjectName = subjectName,
            Status = Status.Available
        };

        var query = new GetCoursesQuery(courseParams);
        var courses = await mediator.Send(query);

        if (!string.IsNullOrWhiteSpace(subjectName))
        {
            ViewBag.SubjectSearch = subjectName;
        }

        if (courses.IsSuccess)
        {
            return View(courses.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Detail(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            return NotFound();
        }

        var query = new GetCourseDetailQuery(id);

        var query1 = new GetRelatedCoursesQuery(id);

        var course = await mediator.Send(query);
        var courses = await mediator.Send(query1);

        if (course.IsSuccess && courses.IsSuccess)
        {
            var courseDetail = course.Value;
            var relatedCourses = courses.Value;

            var courseDetailViewModel = new CourseDetailViewModel()
            {
                CourseDetailDto = courseDetail,
                RelatedCourses = relatedCourses
            };

            return View(courseDetailViewModel);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await PackStaticListToView();
        var query = new GetUserProfileQuery();
        var result = await mediator.Send(query);
        var viewModel = new CourseCreateForLearnerDto();

        if (result.IsSuccess)
        {
            viewModel.LearnerName = result.Value.FirstName + " " + result.Value.LastName;
            viewModel.ContactNumber = result.Value.PhoneNumber;
            viewModel.LearnerGender = result.Value.Gender;
            viewModel.Address = result.Value.City + ", " + result.Value.Country;
        }

        return View(viewModel);
    }


    // POST <CourseController/CreateCourse>
    //[Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCourse(
        CourseCreateForLearnerDto createUpdateCourseDto)
    {
        var result = await mediator.Send(new CreateCourseByLearnerCommand(createUpdateCourseDto));

        return result.IsSuccess
            ? RedirectToAction("SuccessPage", "Home")
            : RedirectToAction("FailPage", "Home");
    }

    [Authorize]
    [HttpPost]
    [Route("request")]
    public async Task<IActionResult> RequestGettingClass(CourseRequestForCreateDto courseRequestForCreateDto)
    {
        var command = new CreateCourseRequestCommand(courseRequestForCreateDto);
        var result = await mediator.Send(command);
        
        if (result.IsFailure)
        {
            return RedirectToAction("FailPage", "Home");
        }

        return RedirectToAction("SuccessRequestPage", "Home");
    }
}