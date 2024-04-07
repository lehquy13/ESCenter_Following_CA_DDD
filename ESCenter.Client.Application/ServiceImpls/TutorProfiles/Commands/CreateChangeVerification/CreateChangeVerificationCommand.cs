using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.CreateChangeVerification;

public record CreateChangeVerificationCommand(List<string> ImageUrls) : ICommandRequest, IAuthorizationRequest;