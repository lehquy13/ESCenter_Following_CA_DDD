using ESCenter.Domain.Shared.Courses;
using ESCenter.Host;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using ESCenter.Mobile.Application.Contracts.Courses.Params;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourse;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;
using ESCenter.Mobile.Application.ServiceImpls.Courses.Queries.GetCourses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class CourseController(
    ILogger<CourseController> logger,
    IMediator mediator)
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
        throw new NotImplementedException();
        // TODO: Create a new query to get course detail, cant use same query as admin
        // var courseById = await mediator.Send(new GetCourseDetailQuery(id));
        //return Ok(courseById);
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
        Guid courseId,
        CourseRequestForCreateDto courseRequestForCreateDto)
    {
        var result = await mediator.Send(new CreateCourseRequestCommand(courseRequestForCreateDto));
        return Ok(result);
    }
}