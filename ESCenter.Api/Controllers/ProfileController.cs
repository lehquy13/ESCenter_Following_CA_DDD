using ESCenter.Application.Accounts.Commands.ChangeAvatar;
using ESCenter.Application.Accounts.Commands.UpdateBasicProfile;
using ESCenter.Application.Accounts.Queries.GetUserProfile;
using ESCenter.Host;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.ReviewCourse;
using ESCenter.Mobile.Application.ServiceImpls.Profiles.Commands.AddOrResetDiscovery;
using ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;
using ESCenter.Mobile.Application.ServiceImpls.Profiles.Queries.GetLearningCourses;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequests;
using ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

[Authorize]
public class ProfileController(
    IAppLogger<ProfileController> logger,
    ISender mediator)
    : ApiControllerBase(logger)
{
    [HttpGet("")]
    public async Task<IActionResult> Profile()
    {
        var loginResult = await mediator.Send(new GetUserProfileQuery());
        return Ok(loginResult);
    }

    [HttpGet("learning-courses")]
    public async Task<IActionResult> GetLearningCourses()
    {
        var result = await mediator.Send(new GetLearningCoursesQuery());
        return Ok(result);
    }

    [Authorize(Roles = "Tutor")]
    [HttpGet("tutor-information")]
    public async Task<IActionResult> GetTutorInformation()
    {
        var result = await mediator.Send(new GetTutorProfileQuery());
        return Ok(result);
    }

    [Authorize(Policy = "RequireTutorRole")]
    [HttpGet("course-requests")]
    public async Task<IActionResult> GetCourseRequests()
    {
        var result = await mediator.Send(new GetCourseRequestsByTutorIdQuery());
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Edit(
        [FromBody] UserProfileUpdateDto learnerForUpdateDto)
    {
        var result = await mediator.Send(new UpdateBasicProfileCommand(learnerForUpdateDto));
        return Ok(result);
    }

    [Authorize]
    [HttpPut("tutor-information/edit")]
    public async Task<IActionResult> EditTutorInformation(
        [FromBody] TutorBasicUpdateForClientDto tutorBasicUpdateDto)
    {
        var result = await mediator.Send(new UpdateTutorInformationCommand(tutorBasicUpdateDto));
        return Ok(result);
    }

    [Authorize(Policy = "RequireTutorRole")]
    [HttpGet("course-request/{courseRequestId:guid}")]
    public async Task<IActionResult> CourseRequestDetail(Guid courseRequestId)
    {
        var requestDetail = await mediator.Send(new GetCourseRequestDetailByTutorIdQuery(courseRequestId));
        return Ok(requestDetail);
    }

    [HttpGet]
    [Route("learning-course/{courseId}")]
    public async Task<IActionResult> GetLearningCourse([FromRoute] string courseId)
    {
        var classInformation = await mediator.Send(new GetLearningCourseDetailQuery(new Guid(courseId)));
        return Ok(classInformation);
    }

    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatar(ChangeAvatarCommand changeAvatarCommand)
    {
        var result = await mediator.Send(changeAvatarCommand);
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [Route("learning-course/{courseId:guid}/review")]
    public async Task<IActionResult> ReviewTutor([FromRoute] Guid courseId,
        [FromBody] ReviewCreateViewModel reviewCreateDto)
    {
        var result = await mediator.Send(new ReviewCourseCommand(
            courseId,
            reviewCreateDto.Rate,
            reviewCreateDto.Detail
        ));
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("add-discovery")]
    public async Task<IActionResult> AddOrResetDiscovery(AddOrResetDiscoveryCommand discoveryCommand)
    {
        var result = await mediator.Send(discoveryCommand);
        return Ok(result);
    }

    [HttpGet]
    [Route("{userId}/get-discovery")]
    public async Task<IActionResult> GetDiscovery(Guid userId)
    {
        await Task.CompletedTask;
        //var result = await mediator.Send(new (userId));
        return Ok("Not implemented yet");
    }
}