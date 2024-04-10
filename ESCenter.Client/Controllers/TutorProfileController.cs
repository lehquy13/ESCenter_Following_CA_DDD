using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Application.Interfaces.Cloudinarys;
using ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetTutorMajors;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.CreateChangeVerification;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;
using ESCenter.Client.Models;
using ESCenter.Client.Utilities;
using ESCenter.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Authorize(Roles = "Tutor")]
[Route("client/[controller]")]
public class TutorProfileController(ISender sender, ICloudinaryServices cloudinaryServices) : Controller
{
    public async Task<IActionResult> Index()
    {
        await PackStaticListToView();
        var getTutorProfile = await sender.Send(new GetTutorProfileQuery());
        var learnerProfile = await sender.Send(new GetUserProfileQuery());

        return View(
            new TutorProfileViewModel
            {
                TutorForProfileDto = getTutorProfile.Value,
                UserProfileDto = learnerProfile.Value
            }
        );
    }

    private async Task PackStaticListToView()
    {
        var subjects = await sender.Send(new GetSubjectsQuery());
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        ViewData["Subjects"] = subjects.Value;
    }

    [HttpPost("edit-tutor-information")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTutorInformation(TutorBasicUpdateForClientDto tutorProfileUpdateDto)
    {
        await PackStaticListToView();

        if (ModelState.IsValid)
        {
            var result = await sender.Send(new UpdateTutorInformationCommand(tutorProfileUpdateDto));

            return result.IsFailure ? Helper.FailResult() : Helper.UpdatedResult();
        }

        return Helper.FailResult();
    }

    [HttpPost("create-change-request")]
    public async Task<IActionResult> CreateChangeRequest(List<IFormFile> fileElems)
    {
        if(fileElems.Count == 0)
        {
            return Helper.FailResult();
        }
        
        var filePath = (from formFile in fileElems
                let fileName = formFile.FileName
                select cloudinaryServices.UploadImage(fileName, formFile.OpenReadStream()))
            .ToList();
        
        var result = await sender.Send(new CreateChangeVerificationCommand(filePath));

        return result.IsFailure ? Helper.FailResult() : Helper.UpdatedResult();
    }
}