using MapsterMapper;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Commands.DeleteSubject;

public record DeleteSubjectCommand(int SubjectId) : ICommandRequest;