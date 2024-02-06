using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Domain.Specifications.Subjects;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Commands.UpsertSubject;

public record UpsertSubjectCommand(SubjectDto SubjectDto) : ICommandRequest;