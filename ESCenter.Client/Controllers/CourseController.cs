using System.Security.Claims;
using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.Contracts.Courses.Params;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourseDetail;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourses;
using ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetRelatedCourses;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Models;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Route("client/[controller]")]
public class CourseController(ISender mediator, IMapper mapper) : Controller
{
    private const int PageSize = 10;

    private async Task PackStaticListToView()
    {
        // ViewData["Roles"] = EnumProvider.Roles;
        // ViewData["Genders"] = EnumProvider.FullGendersOption;
        // ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        // ViewData["LearningModes"] = EnumProvider.LearningModes;
        // ViewData["Statuses"] = EnumProvider.Status;

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

        var course = mediator.Send(query);
        var courses = mediator.Send(query1);

        await Task.WhenAll(course, courses);
        
        if (course.Result.IsSuccess && courses.Result.IsSuccess)
        {
            var courseDetail = course.Result.Value;
            var relatedCourses = courses.Result.Value;

            var courseDetailViewModel = new CourseDetailViewModel()
            {
                CourseForDetailDto = courseDetail,
                RelatedCourses = relatedCourses
            };

            return View(courseDetailViewModel);
        }
        
        return RedirectToAction("Error", "Home");
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        await PackStaticListToView();

        var email = HttpContext.Session.GetString("email");
        if (email is not null)
        {
            var query = new GetLearnerByMailQuery()
            {
                Email = email
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
                return View(mapper.Map<CreateCourseByCustomer>(result.Value));
        }
        //await PackStudentAndTuTorList();

        return View(new CreateCourseByCustomer());
    }


    // POST <CourseController/CreateCourse>
    //[Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateCourse")]
    public async Task<IActionResult> CreateCourse(
        CreateCourseByCustomer createUpdateCourseDto)
    {
        var command = mapper.Map<CreateUpdateCourseCommand>(createUpdateCourseDto);
        command.Email = HttpContext.Session.GetString("email") ?? "";
        var result = await mediator.Send(command);

        return RedirectToAction("SuccessPage", "Home"); //implement
    }


    // PUT api/<CourseController>/5
    [Authorize]
    [HttpPost]
    [Route("UpdateCourse")]
    public async Task<IActionResult> UpdateCourse(
        CreateUpdateCourseDto createUpdateCourseDto)
    {
        var command = mapper.Map<CreateUpdateCourseCommand>(createUpdateCourseDto);

        var result = await mediator.Send(command);

        return Ok(result);
    }


    [HttpPost]
    [Route("RequestGettingClass")]
    public async Task<IActionResult> RequestGettingClass(RequestGettingClassRequest requestGettingClassRequest)
    {
        var tutorId = User.FindFirst(ClaimTypes.Name)?.Value;

        if (User.Identity is null || !User.Identity.IsAuthenticated || tutorId is null)
        {
            string? returnUrl = Url.Action("RequestGettingClass");
            HttpContext.Session.SetString("Value", requestGettingClassRequest.ClassId.ToString());
            return RedirectToAction("Index", "Authentication", new { returnUrl = returnUrl });
        }


        requestGettingClassRequest.TutorId = new(tutorId);
        var command = mapper.Map<RequestGettingClassCommand>(requestGettingClassRequest);
        var result = await mediator.Send(command);
        if (result.IsFailed)
        {
            ViewBag.RequestedMessage = result.Reasons.FirstOrDefault()?.Message ?? "";
            return RedirectToAction("FailPage", "Home");
        }

        return RedirectToAction("SuccessRequestPage", "Home");
    }
}