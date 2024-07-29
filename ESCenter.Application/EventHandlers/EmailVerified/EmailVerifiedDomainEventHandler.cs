using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using MediatR;

namespace ESCenter.Application.EventHandlers.EmailVerified;

public class EmailVerifiedDomainEventHandler(
    IEmailSender emailSender
) : INotificationHandler<EmailVerifiedDomainEvent>
{
    public Task Handle(EmailVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        emailSender.SendEmail(
            "20521318@gm.uit.edu.vn",
            "Email Verified",
            "Your email has been verified successfully. Thank you for using our service."
        );

        return Task.CompletedTask;
    }
}