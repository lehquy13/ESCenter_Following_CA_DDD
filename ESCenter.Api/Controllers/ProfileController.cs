using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Application.Contract.Users.Learners;
using ESCenter.Application.Contract.Users.Tutors;
using ESCenter.Application.ServiceImpls.Accounts.Commands.ChangeAvatar;
using ESCenter.Application.ServiceImpls.Accounts.Commands.CreateUpdateLearnerProfile;
using ESCenter.Application.ServiceImpls.Accounts.Commands.UpdateTutorProfile;
using ESCenter.Application.ServiceImpls.Accounts.Queries.GetTutorProfile;
using ESCenter.Application.ServiceImpls.Accounts.Queries.GetUserProfile;
using ESCenter.Application.ServiceImpls.Clients.Courses.Commands.ReviewCourse;
using ESCenter.Application.ServiceImpls.Clients.Profiles.Commands.AddOrResetDiscovery;
using ESCenter.Application.ServiceImpls.Clients.Profiles.Queries.GetLearningCourses;
using ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetCourseRequestDetail;
using ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetCourseRequests;
using ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetLearningCourse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers
{
    [Authorize]
    public class ProfileController(
        ILogger<ProfileController> logger,
        IMediator mediator)
        : ApiControllerBase(logger)
    {
        [HttpGet("")]
        public async Task<IActionResult> Profile()
        {
            var loginResult = await mediator.Send(new GetUserProfileQuery());

            return Ok(loginResult);
        }

        [HttpGet("/learning-courses")]
        public async Task<IActionResult> GetLearningCourses()
        {
            var result = await mediator.Send(new GetLearningCoursesQuery());

            return Ok(result);
        }

        [HttpGet("/tutor-information")]
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
        public async Task<IActionResult> Edit([FromBody] LearnerForCreateUpdateDto learnerForCreateUpdateDto)
        {
            var result = await mediator.Send(new CreateUpdateLearnerProfileCommand(learnerForCreateUpdateDto));
            return Ok(result);
        }

        [Authorize]
        [HttpPut("tutor-information/edit")]
        public async Task<IActionResult> EditTutorInformation(
            [FromBody] TutorBasicForUpdateDto tutorBasicForUpdateDto)
        {
            var result = await mediator.Send(new UpdateTutorProfileCommand(tutorBasicForUpdateDto));

            return Ok(result);
        }

        [Authorize(Policy = "RequireTutorRole")]
        [HttpGet("course-request/{courseRequestId}")]
        public async Task<IActionResult> CourseRequestDetail(Guid courseRequestId)
        {
            var requestDetail = await mediator.Send(new GetCourseRequestDetailQuery(courseRequestId));
            return Ok(requestDetail);
        }

        [HttpGet]
        [Route("learning-course/{courseId}")]
        public async Task<IActionResult> GetLearningCourse(GetLearningCourseDetailQuery getLearningCourseDetail)
        {
            var classInformation = await mediator.Send(getLearningCourseDetail);

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
        [Route("learning-course/{courseId}/review")]
        public async Task<IActionResult> ReviewTutor([FromRoute] Guid courseId,
            [FromBody] ReviewDetailDto reviewDetailDto)
        {
            var result = await mediator.Send(new ReviewCourseCommand(reviewDetailDto));
            return Ok(); //implement
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
        public async Task<IActionResult> GetDiscovery(int userId)
        {
            //var result = await mediator.Send(new (userId));
            return Ok("Not implemented yet");
        }
    }
}