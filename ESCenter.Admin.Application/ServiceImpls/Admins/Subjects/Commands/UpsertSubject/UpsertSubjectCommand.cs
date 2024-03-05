using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Commands.UpsertSubject;

public record UpsertSubjectCommand(SubjectDto SubjectDto) : ICommandRequest;