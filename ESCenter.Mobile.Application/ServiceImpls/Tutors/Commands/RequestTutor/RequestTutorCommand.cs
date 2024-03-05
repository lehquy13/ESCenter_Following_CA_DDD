using ESCenter.Mobile.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Tutors.Commands.RequestTutor;

public record RequestTutorCommand(TutorRequestForCreateDto TutorRequestForCreateDto) : ICommandRequest;