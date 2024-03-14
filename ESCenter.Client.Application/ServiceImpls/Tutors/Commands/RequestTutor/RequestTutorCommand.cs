using ESCenter.Client.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public record RequestTutorCommand(TutorRequestForCreateDto TutorRequestForCreateDto) : ICommandRequest;