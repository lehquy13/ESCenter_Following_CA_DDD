using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Tutors.Commands.ClearTutorRequests;

public record ClearTutorRequestsCommand(Guid TutorId) : ICommandRequest;