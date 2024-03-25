using System.Security.Claims;
using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.Contracts.Users.Params;
using ESCenter.Client.Application.Contracts.Users.Tutors;
using ESCenter.Client.Application.ServiceImpls.Courses.Commands.ReviewCourse;
using ESCenter.Client.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutors;
using ESCenter.Client.Models;
using ESCenter.Client.Utilities;
using ESCenter.Domain.Shared.Courses;
using Matt.Paginated;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ESCenter.Client.Controllers;

[Route("client/[controller]")]
public class TutorController(ISender mediator, IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private readonly int _pageSize = 12;

    private async Task PackStaticListToView()
    {
        ViewData["Subjects"] = await mediator.Send(new GetSubjectsQuery());
    }

    // Query
    // GET: api/<TutorInformationController>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index(
        int pageIndex = 1,
        string subjectName = "",
        int birthYear = 0,
        string gender = "None",
        string academicLevel = "Optional",
        string address = "")
    {
        await PackStaticListToView();

        var tutorParams = new TutorParams
        {
            PageSize = _pageSize,
            PageIndex = pageIndex,
            SubjectName = subjectName,
            BirthYear = birthYear,
            Gender = gender,
            Academic = academicLevel,
            Address = address
        };

        var query = new GetTutorsQuery(tutorParams);
        var tutorDtos = await mediator.Send(query);

        if (tutorDtos.IsSuccess)
        {
            ViewData["TutorParams"] = tutorParams;
            return View(tutorDtos.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Detail(Guid id)
    {
        var query = new GetTutorDetailQuery(id);
        var tutorDto = await mediator.Send(query);

        return View(tutorDto.Value);
    }

    [Authorize]
    [HttpGet]
    [Route("tutor-registration")]
    public async Task<IActionResult> TutorRegistration()
    {
        //Get userId through User.Identity
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrWhiteSpace(id))
        {
            return RedirectToAction("Login", "Authentication");
        }

        var query = new GetUserProfileQuery();
        var result = await mediator.Send(query);

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    // POST <TutorInformationController>
    [Authorize]
    [HttpPost]
    [Route("tutor-registration")]
    public async Task<IActionResult> TutorRegistration(TutorRegistrationDto tutorForDetailDto)
    {
        // if (filePaths != null)
        // {
        //     for (var i = 0; i < filePaths.Count; i++)
        //     {
        //         filePaths[i] = webHostEnvironment.WebRootPath + filePaths.ElementAt(i);
        //     }
        // }
        //
        // //Handle filepath !!! Upgrade
        // //alternative flows: get the verification by id then push into verification list of tutor
        // if (filePaths is { Count: > 0 })
        // {
        //     foreach (var i in filePaths)
        //     {
        //         tutorForDetailDto.TutorVerificationInfoDtos.Add(new TutorVerificationInfoDto
        //         {
        //             Image = i,
        //             TutorId = tutorForDetailDto.Id
        //         });
        //     }
        // }
        
        var command = new RegisterAsTutorCommand(tutorForDetailDto);

        var result = await mediator.Send(command);

        Helper.ClearTempFile(webHostEnvironment.WebRootPath);

        if (result.IsSuccess)
        {
            HttpContext.Session.SetString("role", UserRole.Tutor.ToString());
            return RedirectToAction("SuccessPage", "Home");
        }

        return RedirectToAction("FailPage", "Home");
    }
}