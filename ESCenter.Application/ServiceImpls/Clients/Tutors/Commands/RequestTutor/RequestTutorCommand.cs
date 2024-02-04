using ESCenter.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Clients.Tutors.Commands.RequestTutor;

public record RequestTutorCommand(TutorRequestForCreateDto TutorRequestForCreateDto) : ICommandRequest;