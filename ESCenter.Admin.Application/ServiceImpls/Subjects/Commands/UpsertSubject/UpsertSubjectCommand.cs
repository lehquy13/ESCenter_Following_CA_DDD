using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects.Commands.UpsertSubject;

public record UpsertSubjectCommand(SubjectDto SubjectDto) : ICommandRequest;

public class UpsertSubjectCommandValidator : AbstractValidator<UpsertSubjectCommand>
{
    public UpsertSubjectCommandValidator()
    {
        RuleFor(x => x.SubjectDto).NotNull();
        RuleFor(x => x.SubjectDto).SetValidator(new SubjectDtoValidator());
    }
}