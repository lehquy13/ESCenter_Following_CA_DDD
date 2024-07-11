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
            notification.Customer.Email,
            "Email Verified",
            "Your email has been verified successfully. Thank you for using our service."
        );

        return Task.CompletedTask;
    }
}