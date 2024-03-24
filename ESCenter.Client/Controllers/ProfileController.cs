using ESCenter.Application.Accounts.Commands.ChangePassword;
using ESCenter.Application.Accounts.Commands.CreateUpdateBasicProfile;
using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Application.Accounts.Queries.Login;
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
    ISender mediator,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private async Task PackStaticListToView()
    {
        var subjects = await mediator.Send(new GetSubjectsQuery());
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        ViewData["Subjects"] = subjects.Value;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        await PackStaticListToView();

        var query = new GetUserProfileQuery();
        var loginResult = await mediator.Send(query);

        if (loginResult.IsSuccess)
        {
            var viewModelResult = new ProfileViewModel
            {
                UserProfileDto = loginResult.Value
            };

            // if (loginResult.Value.Role == UserRole.Tutor)
            // {
            //     var query1 = new GetTutorProfileQuery()
            //     {
            //         ObjectId = loginResult.Value.Id
            //     };
            //     var loginResult1 = await mediator.Send(query1);
            //
            //     viewModelResult.RequestGettingClassDtos = loginResult1.Value.RequestGettingClassForListDtos;
            //     viewModelResult.TutorDto = loginResult1.Value.TutorMainInfoDto;
            // }

            return View(viewModelResult);
        }

        return RedirectToAction("Login", "Authentication", new LoginQuery("", ""));
    }

    [HttpPost("ChooseProfilePictures")]
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

    [HttpPost("ChoosePicture")]
    public async Task<IActionResult> ChoosePicture(IFormFile? formFile)
    {
        if (formFile == null)
        {
            return Json(false);
        }

        var image = await Helper.SaveFiles(formFile, webHostEnvironment.WebRootPath);

        return Json(new { res = true, image = "temp\\" + Path.GetFileName(image) });
    }

    [HttpPost("edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserProfileUpdateDto userProfileUpdateDto, IFormFile? formFile)
    {
        await PackStaticListToView();

        if (!ModelState.IsValid)
        {
            return Helper.RenderRazorViewToString(this, "_ProfileEdit",
                userProfileUpdateDto,
                true
            );
        }
        // var filePath = string.Empty;
        // if (formFile != null)
        // {
        //     filePath = await Helper.SaveFiles(formFile, webHostEnvironment.WebRootPath);
        // }

        var result = await mediator.Send(new UpdateBasicProfileCommand(userProfileUpdateDto));
        //ViewBag.Updated = result.IsSuccess;
        //Helper.ClearTempFile(webHostEnvironment.WebRootPath);

        if (result.IsSuccess)
        {
            HttpContext.Session.SetString("name",
                userProfileUpdateDto.FirstName + " " + userProfileUpdateDto.LastName);
            HttpContext.Session.SetString("image", userProfileUpdateDto.Avatar);

            // if (userDto.Role == UserRole.Tutor)
            // {
            //     var query1 = new GetTutorProfileQuery()
            //     {
            //         ObjectId = userDto.Id
            //     };
            //     var loginResult1 = await mediator.Send(query1);
            //
            //     viewModelResult.RequestGettingClassDtos = loginResult1.Value.RequestGettingClassForListDtos;
            //     viewModelResult.TutorDto = loginResult1.Value.TutorMainInfoDto;
            // }
        }

        return Helper.UpdatedResult();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        if (!ModelState.IsValid)
        {
            return Helper.RenderRazorViewToString(this, "_ChangePassword", changePasswordRequest, true);
        }

        var loginResult = await mediator.Send(
            new ChangePasswordCommand(
                changePasswordRequest.CurrentPassword,
                changePasswordRequest.NewPassword,
                changePasswordRequest.ConfirmPassword));

        if (loginResult.IsSuccess)
        {
            return Helper.UpdatedResult();
        }

        return Helper.FailResult();
    }

    [HttpGet]
    [Route("request-detail/{courseId:guid}")]
    public async Task<IActionResult> TeachingClassDetail(Guid courseId)
    {
        var query = new GetCourseRequestDetailQuery(courseId);
        var course = await mediator.Send(query);

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
        var course = await mediator.Send(query);
        
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

        var result = await mediator.Send(command);

        return RedirectToAction("Index");
    }
}