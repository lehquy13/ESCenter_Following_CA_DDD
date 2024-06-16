using ESCenter.Mobile.Application.Contracts.Users.Params;
using ESCenter.Mobile.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;
using ESCenter.Mobile.Application.ServiceImpls.Tutors.Commands.RequestTutor;
using ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutorDetail;
using ESCenter.Mobile.Application.ServiceImpls.Tutors.Queries.GetTutors;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class TutorController(
    IAppLogger<TutorController> logger,
    ISender mediator)
    : ApiControllerBase(logger)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllTutors([FromQuery] TutorParams tutorParams)
    {
        var tutorDtos = await mediator.Send(new GetTutorsQuery(tutorParams));
        return Ok(tutorDtos);
    }
    
    [HttpGet]
    [Route("by-ids")]
    public async Task<IActionResult> GetTutorsByIds([FromBody] List<Guid> tutorIds)
    {
        var tutorDtos = await mediator.Send(new GetTutorsByIdsQuery(tutorIds));
        return Ok(tutorDtos);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTutor(Guid id)
    {
        var tutorDto = await mediator.Send(new GetTutorDetailQuery(id));
        return Ok(tutorDto);
    }

    // POST api/<TutorController>/Register 
    [Authorize(Roles = "Learner")]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> TutorRegistration(
        [FromBody] TutorRegistrationDto tutorBasicForRegisterCommand)
    {
        var result = await mediator.Send(new RegisterAsTutorCommand(tutorBasicForRegisterCommand));
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("{tutorId}/request-tutor")]
    public async Task<IActionResult> RequestTutor(Guid tutorId, RequestTutorRequest requestTutorRequest)
    {
        var result = await mediator.Send(new RequestTutorCommand(tutorId, requestTutorRequest.RequestMessage));
        return Ok(result);
    }
}