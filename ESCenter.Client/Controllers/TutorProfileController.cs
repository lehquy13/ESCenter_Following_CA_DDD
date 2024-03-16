using ESCenter.Client.Application.Contracts.Users.Tutors;
using ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetTutorMajors;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;
using ESCenter.Client.Utilities;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Client.Controllers;

[Authorize]
[Route("client/[controller]")]
public class TutorProfileController(ISender mediator) : Controller
{
    private async Task PackStaticListToView()
    {
        var subjects = await mediator.Send(new GetSubjectsQuery());
        ViewData["Roles"] = EnumProvider.Roles;
        ViewData["Genders"] = EnumProvider.Genders;
        ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        ViewData["Subjects"] = subjects.Value;
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> GetTutorMajors()
    {
        var query = new GetTutorMajorsQuery();
        var result = await mediator.Send(query);

        return Helper.RenderRazorViewToString(this, "_TutorMajors", result.Value);
    }

    [HttpPost("edit-tutor-information")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTutorInformation(TutorBasicUpdateForClientDto tutorProfileUpdateDto)
    {
        await PackStaticListToView();

        if (ModelState.IsValid)
        {
            //var filePath = new List<string>();
            // if (files != null)
            // {
            //     foreach (var i in files)
            //     {
            //         if (i is not null)
            //             filePath.Add(await Helper.SaveFiles(i, webHostEnvironment.WebRootPath));
            //     }
            // }
            //
            // if (tutorProfileUpdateDto.Role != UserRole.Tutor)
            // {
            //     throw new Exception("User has not registered as Tutor.");
            // }

            var result = await mediator.Send(new UpdateTutorInformationCommand(tutorProfileUpdateDto));

            //ViewBag.Updated = result.IsSuccess;
            //Helper.ClearTempFile(webHostEnvironment.WebRootPath);
            
            return result.IsFailure ? Helper.FailResult() : Helper.UpdatedResult();
        }

        return Helper.RenderRazorViewToString(this, "_ProfileEdit",
            tutorProfileUpdateDto,
            true
        );
    }
}