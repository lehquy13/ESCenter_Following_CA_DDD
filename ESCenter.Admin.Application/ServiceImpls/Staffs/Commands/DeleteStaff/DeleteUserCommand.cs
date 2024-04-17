using FluentValidation;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Commands.DeleteStaff;

public record DeleteStaffCommand(Guid Id) : ICommandRequest;

public class DeleteStaffCommandValidator : AbstractValidator<DeleteStaffCommand>
{
    public DeleteStaffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}