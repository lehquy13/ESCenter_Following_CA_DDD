﻿using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public record RequestTutorCommand(Guid TutorId, string RequestMessage) : ICommandRequest, IAuthorizationRequest;

public class RequestTutorCommandValidator : AbstractValidator<RequestTutorCommand>
{
    public RequestTutorCommandValidator()
    {
        RuleFor(x => x.TutorId).NotEmpty();
        RuleFor(x => x.RequestMessage).NotEmpty();
    }
}