using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Application.Interfaces.Cloudinarys;
using ESCenter.Client.Application.ServiceImpls.Payments.Commands.MakePayment;
using ESCenter.Client.Application.ServiceImpls.Payments.Queries.Get;
using ESCenter.Client.Application.ServiceImpls.Payments.Queries.Gets;
using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.CreateChangeVerification;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;
using ESCenter.Client.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;
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
        var paymentDtos = await sender.Send(new GetAllPaymentsQuery());

        return View(
            new TutorProfileViewModel
            {
                TutorForProfileDto = getTutorProfile.Value,
                UserProfileDto = learnerProfile.Value,
                PaymentDtos = paymentDtos.Value
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

        if (!ModelState.IsValid) return Helper.FailResult();

        var result = await sender.Send(new UpdateTutorInformationCommand(tutorProfileUpdateDto));

        return result.IsFailure ? Helper.FailResult() : Helper.UpdatedResult();
    }


    [HttpGet]
    [Route("request-detail/{courseId:guid}")]
    public async Task<IActionResult> TeachingClassDetail(Guid courseId)
    {
        var query = new GetCourseRequestDetailQuery(courseId);
        var course = await sender.Send(query);

        return course.IsSuccess
            ? Helper.RenderRazorViewToString(this, "_CourseRequestDetailPartialView", course.Value)
            : Helper.FailResult();
    }

    [HttpPost("create-change-request")]
    public async Task<IActionResult> CreateChangeRequest(List<IFormFile> fileElems)
    {
        if (fileElems.Count == 0)
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

    [HttpPost("payment/{id:guid}/set-as-paid")]
    public async Task<IActionResult> SetAsPaid(Guid id)
    {
        var result = await sender.Send(new MakePaymentCommand(id));

        return result.IsSuccess
            ? Helper.SuccessResult("Payment has been made successfully.")
            : Helper.FailResult("Payment failed. Please try again later.");
    }

    [HttpGet("payment/{id:guid}")]
    public async Task<IActionResult> MakePayment(Guid id)
    {
        var paymentInfo = await sender.Send(new GetPaymentDetailQuery(id));

        if (paymentInfo.IsFailure)
        {
            return Helper.FailResult();
        }

        var payment = new PaymentModel
        (
            id,
            paymentInfo.Value.Code,
            paymentInfo.Value.TutorName
        );

        return Helper.RenderRazorViewToString(this, "_MakePayment", payment);
    }
}