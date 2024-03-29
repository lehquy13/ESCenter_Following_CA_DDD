using ESCenter.Domain.Shared.Courses;
using ESCenter.Host;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.Contracts.Courses.Params;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourse;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;
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
    // GET: api/<CourseController>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllCourses([FromQuery] CourseParams courseParams)
    {
        courseParams.Status = Status.Available;
        var courseResult = await mediator.Send(new GetCoursesQuery(courseParams));
        return Ok(courseResult);
    }

    // GET api/<CourseController>/5
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetCourse(Guid id)
    {
        var courseById = await mediator.Send(new GetCourseDetailQuery(id));
        return Ok(courseById);
    }

    // POST api/Create/<CourseController>
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCourse(
        CourseForLearnerCreateDto courseForLearnerCreateDto)
    {
        var creatingResult = await mediator.Send(new CreateCourseByLearnerCommand(courseForLearnerCreateDto));
        return Ok(creatingResult);
    }


    // POST api/<CourseController>/RequestCourse
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
}