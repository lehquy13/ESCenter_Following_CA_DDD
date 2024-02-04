using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.Contracts.Users.Params;
using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorDetail;
using ESCenter.Application.ServiceImpls.Clients.Tutors.Commands.RequestTutor;
using ESCenter.Application.ServiceImpls.Clients.Tutors.GetTutors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ESCenter.Api.Controllers;

public class TutorInformationController(
    ILogger<TutorInformationController> logger,
    IMediator mediator)
    : ApiControllerBase(logger)
{
    // Query
    // GET: api/<TutorInformation>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllTutors([FromQuery] TutorParams tutorParams)
    {
        var tutorDtos = await mediator.Send(new GetTutorsQuery(tutorParams));
        return Ok(tutorDtos);
    }


    // GET api/<TutorInformationController>/5
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTutor(Guid id)
    {
        var tutorDto = await mediator.Send(new GetTutorDetailQuery(id));
        return Ok(tutorDto);
    }

    // TODO: test
    // POST api/<TutorInformationController>/Register 
    [Authorize(Roles = "Learner")]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> TutorRegistration(
        [FromBody] TutorBasicForRegisterCommand tutorBasicForRegisterCommand)
    {
        var result = await mediator.Send(tutorBasicForRegisterCommand);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("{tutorId}/request-tutor")]
    public async Task<IActionResult> RequestTutor(int tutorId,
        [FromBody] TutorRequestForCreateDto tutorRequestForCreateDto)
    {
        var result = await mediator.Send(new RequestTutorCommand(tutorRequestForCreateDto));
        return Ok(result);
    }
}