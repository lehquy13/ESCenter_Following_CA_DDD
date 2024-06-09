using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Application.Interfaces.Cloudinarys;
using ESCenter.Client.Application.Contracts.Users.Params;
using ESCenter.Client.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.Tutors.Commands.RequestTutor;
using ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Client.Application.ServiceImpls.Tutors.Queries.GetTutors;
using ESCenter.Client.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ESCenter.Client.Controllers;

[Route("client/[controller]")]
public class TutorController(
    ISender mediator,
    IWebHostEnvironment webHostEnvironment,
    ICloudinaryServices cloudinaryServices)
    : Controller
{
    private readonly int _pageSize = 12;

    private async Task PackStaticListToView()
    {
        var subjects = await mediator.Send(new GetSubjectsQuery());
        ViewData["Subjects"] = subjects.Value;
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
        await PackStaticListToView();
        var query = new GetUserProfileQuery();
        var result = await mediator.Send(query);

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [Authorize]
    [HttpPost]
    [Route("{tutorId:guid}/request-tutor")]
    public async Task<IActionResult> RequestTutor([FromRoute] Guid tutorId,
        string requestMessage)
    {
        var result = await mediator.Send(new RequestTutorCommand(tutorId, requestMessage));

        return RedirectToAction(result.IsSuccess ? "SuccessPage" : "Error", "Home");
    }

    [Authorize]
    [HttpPost]
    [Route("tutor-registration")]
    public async Task<IActionResult> TutorRegistration(
        TutorRegistrationDto tutorForDetailDto,
        List<IFormFile> imageFileUrls)
    {
        tutorForDetailDto.ImageFileUrls = [];

        foreach (var formFile in imageFileUrls)
        {
            if (formFile.Length <= 0)
            {
                return BadRequest();
            }

            var fileName = formFile.FileName;
            await using var fileStream = formFile.OpenReadStream();

            var uploadResult = cloudinaryServices.UploadImage(fileName, fileStream);

            tutorForDetailDto.ImageFileUrls.Add(uploadResult);
        }

        var command = new RegisterAsTutorCommand(tutorForDetailDto);
        var result = await mediator.Send(command);

        Helper.ClearTempFile(webHostEnvironment.WebRootPath);

        if (result.IsFailure) return RedirectToAction("FailPage", "Home");

        HttpContext.Session.Clear();
        return RedirectToAction("SuccessRequestPage", "Home");
    }

    [HttpPost("choose-profile-pictures")]
    public async Task<IActionResult> ChooseProfilePictures(List<IFormFile?> formFiles)
    {
        if (formFiles.Count <= 0)
        {
            return Json(false);
        }

        var values = new List<string>();
        foreach (var i in formFiles)
        {
            var image = await Helper.SaveFiles(i, webHostEnvironment.WebRootPath);
            values.Add("\\temp\\" + Path.GetFileName(image));
        }

        return Json(new { res = true, images = values });
    }
}