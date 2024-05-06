using System.Runtime.InteropServices.JavaScript;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.TutorRequests.Commands.MarkRequestAsDone;

public record MarkRequestAsDoneCommand(Guid RequestId) : ICommandRequest;

public static class TutorRequestAppServiceError
{
    public static Error RequestNotFound = new Error("RequestNotFound", "Request not found");
}