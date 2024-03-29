using ESCenter.Host;
using ESCenter.Mobile.Application.ServiceImpls.Subjects.Queries.GetSubject;
using ESCenter.Mobile.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Api.Controllers;

public class SubjectController(
    IAppLogger<SubjectController> logger,
    ISender mediator)
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