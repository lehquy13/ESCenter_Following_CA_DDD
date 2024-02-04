using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.ClearTutorRequests;

public record ClearTutorRequestsCommand(Guid TutorId) : ICommandRequest;