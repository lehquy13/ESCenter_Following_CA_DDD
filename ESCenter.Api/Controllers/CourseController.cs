using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.Contracts.Courses.Params;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourse;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.PurchaseCourse;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Queries.GetCourseDetail;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Queries.GetCourses;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class CourseController(
    IAppLogger<CourseController> logger,
    ISender mediator)
    : ApiControllerBase(logger)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllCourses([FromQuery] CourseParams courseParams)
    {
        var courseResult = await mediator.Send(new GetCoursesQuery(courseParams));
        return Ok(courseResult);
    }
    
        
    [HttpPost]
    [Route("by-ids")]
    public async Task<IActionResult> GetCoursesByIds([FromBody] List<Guid> courseIds)
    {
        var courses = await mediator.Send(new GetCoursesByIdsQuery(courseIds));
        return Ok(courses);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetCourse(Guid id)
    {
        var courseById = await mediator.Send(new GetCourseDetailQuery(id));
        return Ok(courseById);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateCourse(
        CourseForLearnerCreateDto courseForLearnerCreateDto)
    {
        var creatingResult = await mediator.Send(new CreateCourseByLearnerCommand(courseForLearnerCreateDto));
        return Ok(creatingResult);
    }

    [Authorize(Policy = "RequireTutorRole")]
    [HttpPut]
    [Route("{courseId}/request-course")]
    public async Task<IActionResult> RequestCourse(
        [FromRoute] Guid courseId, Guid tutorId)
    {
        var courseRequestForCreateDto = new CourseRequestForCreateDto(courseId, tutorId);
        var result = await mediator.Send(new CreateCourseRequestCommand(courseRequestForCreateDto));
        return Ok(result);
    }

    /// <summary>
    /// Deprecated
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [Authorize(Policy = "RequireTutorRole")]
    [HttpPut]
    [Route("{courseId:guid}/purchase-course")]
    public async Task<IActionResult> PurchaseCourse([FromRoute] Guid courseId)
    {
        var purchaseCourseCommand = new PurchaseCourseCommand(courseId);
        var result = await mediator.Send(purchaseCourseCommand);

        return Ok(result);
    }
}