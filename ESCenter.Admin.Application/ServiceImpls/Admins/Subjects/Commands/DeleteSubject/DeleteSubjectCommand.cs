using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Commands.DeleteSubject;

public record DeleteSubjectCommand(int SubjectId) : ICommandRequest;