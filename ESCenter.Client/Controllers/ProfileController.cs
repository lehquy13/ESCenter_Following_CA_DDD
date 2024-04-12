using ESCenter.Application.Accounts.Commands.ChangeAvatar;
using ESCenter.Application.Accounts.Commands.ChangePassword;
using ESCenter.Application.Accounts.Commands.UpdateBasicProfile;
using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Application.Accounts.Queries.Login;
using ESCenter.Application.Interfaces.Cloudinarys;
using ESCenter.Client.Application.ServiceImpls.Courses.Commands.ReviewCourse;
using ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;
using ESCenter.Client.Models;
using ESCenter.Client.Utilities;
using ESCenter.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Authorize]
[Route("client/[controller]")]
public class ProfileController(
    ISender sender,
    ILogger<ProfileController> logger,
    ICloudinaryServices cloudinaryServices,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private async Task PackStaticListToView()
    {
        var subjects = await sender.Send(new GetSubjectsQuery());
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        ViewData["Subjects"] = subjects.Value;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        await PackStaticListToView();

        var learnerProfile = await sender.Send(new GetUserProfileQuery());

        if (learnerProfile is { IsSuccess: true, Value: not null })
        {
            return View(new ProfileViewModel
            {
                UserProfileDto = learnerProfile.Value,
            });
        }

        return RedirectToAction("Index", "Authentication", new LoginQuery("", ""));
    }

    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatar(IFormFile? formFile)
    {
        if (formFile is null || formFile.Length <= 0)
        {
            return BadRequest();
        }

        var fileName = formFile.FileName;
        await using var fileStream = formFile.OpenReadStream();

        var result = cloudinaryServices.UploadImage(fileName, fileStream);

        var changePictureResult = await sender.Send(new ChangeAvatarCommand(result));

        if (!changePictureResult.IsSuccess)
        {
            return BadRequest();
        }

        HttpContext.Session.SetString("image", result);
        return Json(new { res = true, image = result });
    }

    [HttpPost("choose-picture")]
    public async Task<IActionResult> ChoosePicture(IFormFile? formFile)
    {
        if (formFile == null)
        {
            return Json(false);
        }

        var image = await Helper.SaveFiles(formFile, webHostEnvironment.WebRootPath);

        return Json(new { res = true, image = "/temp/" + Path.GetFileName(image) });
    }

    [HttpPost("edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserProfileUpdateDto userDto) //, IFormFile? formFile)
    {
        await PackStaticListToView();

        if (!ModelState.IsValid)
        {
            return Helper.FailResult();
        }

        var result = await sender.Send(new UpdateBasicProfileCommand(userDto));

        try
        {
            Helper.ClearTempFile(webHostEnvironment.WebRootPath);
        }
        catch (Exception)
        {
            logger.LogError("Temp folder does not exist");
        }

        if (result is { IsSuccess: true, Value: not null })
        {
            HttpContext.Session.SetString("name", result.Value.User.FullName);
            HttpContext.Session.SetString("image", result.Value.User.Avatar);

            return Helper.UpdatedResult();
        }

        return Helper.FailResult();
    }

    [HttpPost("change-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        if (!ModelState.IsValid)
        {
            return Helper.FailResult();
        }

        try
        {
            var loginResult = await sender.Send(new ChangePasswordCommand(
                changePasswordRequest.CurrentPassword,
                changePasswordRequest.NewPassword,
                changePasswordRequest.ConfirmedPassword));

            if (loginResult.IsSuccess)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Authentication");
            }
        }
        catch (Exception ex)
        {
            //Log the error (uncomment ex variable name and write a log.)
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " + ex.Message +
                                         "see your system administrator.");
        }

        return Helper.FailResult();
    }

    [HttpGet]
    [Route("request-detail/{courseId:guid}")]
    public async Task<IActionResult> TeachingClassDetail(Guid courseId)
    {
        var query = new GetCourseRequestDetailQuery(courseId);
        var course = await sender.Send(query);

        if (course.IsSuccess)
        {
            return Helper.RenderRazorViewToString(this, "_TeachingClassDetail", course.Value);
        }

        return RedirectToAction("Error", "Home");
    }

    [HttpGet]
    [Route("learning-course/{courseId:guid}")]
    public async Task<IActionResult> GetLearningClass(Guid courseId)
    {
        var query = new GetLearningCourseDetailQuery(courseId);
        var course = await sender.Send(query);

        if (course.IsSuccess)
        {
            return Helper.RenderRazorViewToString(this, "_LearningClassDetail", course.Value);
        }

        return RedirectToAction("Error", "Home");
    }


    [Authorize]
    [HttpPost]
    [Route("learning-course/{courseId:guid}/review-tutor")]
    public async Task<IActionResult> ReviewTutor(Guid courseId, ReviewCourseViewModel reviewCourseView)
    {
        var command = new ReviewCourseCommand(courseId, reviewCourseView.Detail, reviewCourseView.Rate);

        var result = await sender.Send(command);

        return RedirectToAction("Index");
    }
}