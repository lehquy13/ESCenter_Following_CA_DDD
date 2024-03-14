using System.Security.Claims;
using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Client.Application.Contracts.Users.Params;
using ESCenter.Client.Application.Contracts.Users.Tutors;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutors;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.Paginated;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ESCenter.Client.Controllers;

[Route("[controller]")]
public class TutorController(ISender mediator, IWebHostEnvironment webHostEnvironment, IMapper mapper)
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
    public async Task<IActionResult> Index(int pageIndex = 1, string subjectName = "", int birthYear = 0,
        string gender = "", string academicLevel = "", string address = "")
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
    [Route("TutorRegistration")]
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
    [Route("TutorRegistration")]
    public async Task<IActionResult> TutorRegistration(TutorBasicForRegisterCommand tutorForDetailDto,
        List<string>? subjectId, List<string>? filePaths)
    {
        //this line will be removed soon
        if (subjectId is not null)
            tutorForDetailDto.Majors = subjectId;

        if (filePaths != null)
        {
            for (var i = 0; i < filePaths.Count; i++)
            {
                filePaths[i] = webHostEnvironment.WebRootPath + filePaths.ElementAt(i);
            }
        }

        //Handle filepath !!! Upgrade
        //alternative flows: get the verification by id then push into verification list of tutor
        if (filePaths is { Count: > 0 })
        {
            foreach (var i in filePaths)
            {
                tutorForDetailDto.TutorVerificationInfoDtos.Add(new TutorVerificationInfoDto
                {
                    Image = i,
                    TutorId = tutorForDetailDto.Id
                });
            }
        }

        var command = new TutorRegistrationCommand(tutorForDetailDto);

        var result = await mediator.Send(command);

        Helper.ClearTempFile(webHostEnvironment.WebRootPath);
        if (result.IsSuccess)
        {
            HttpContext.Session.SetString("role", UserRole.Tutor.ToString());
            return RedirectToAction("SuccessPage", "Home");
        }

        return RedirectToAction("FailPage", "Home");
    }

    [Authorize]
    [HttpPost]
    [Route("ReviewTutor")]
    public async Task<IActionResult> ReviewTutor(TutorReviewRequestDto tutorReviewRequestDto)
    {
        var command = new CreateReviewCommand
        {
            ReviewDto = new TutorReviewDto()
            {
                Rate = tutorReviewRequestDto.Rate,
                Description = tutorReviewRequestDto.Description,
                Id = tutorReviewRequestDto.Id,
                ClassInformationId = new Guid(tutorReviewRequestDto.ClassId)
            },
            LearnerEmail = User.FindFirstValue(ClaimTypes.Email) ?? "",
            TutorEmail = tutorReviewRequestDto.TutorEmail,
        };

        var result = await mediator.Send(command);

        return RedirectToAction("Index"); //implement
    }
}