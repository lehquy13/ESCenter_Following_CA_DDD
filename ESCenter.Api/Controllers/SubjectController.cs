using ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubject;
using ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class SubjectController(
    ILogger<SubjectController> logger,
    IMediator mediator)
    : ApiControllerBase(logger)
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await mediator.Send(new GetSubjectsQuery());
        return Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        var subject = await mediator.Send(new GetSubjectQuery(id));
        return Ok(subject);
    }
}