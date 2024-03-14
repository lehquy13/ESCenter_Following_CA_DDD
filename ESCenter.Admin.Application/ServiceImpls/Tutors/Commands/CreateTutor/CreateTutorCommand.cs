using ESCenter.Admin.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.CreateTutor;

public record CreateTutorCommand(TutorCreateDto TutorForCreateDto) : ICommandRequest;