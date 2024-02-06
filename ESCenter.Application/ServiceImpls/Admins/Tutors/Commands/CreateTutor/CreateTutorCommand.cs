using ESCenter.Application.Contract.Users.Tutors;
using ESCenter.Domain.Aggregates.Users.Errors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.CreateTutor;

public record CreateTutorCommand(TutorCreateUpdateDto TutorForCreateUpdateDto) : ICommandRequest;