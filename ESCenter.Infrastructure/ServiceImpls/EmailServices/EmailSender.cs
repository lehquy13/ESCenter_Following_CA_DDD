using System.Net;
using System.Net.Mail;
using ESCenter.Application.Contract.Interfaces.Emails;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Options;

namespace ESCenter.Infrastructure.ServiceImpls.EmailServices;

public class EmailSender : IEmailSender
{
    private readonly EmailSettingNames _emailSettingNames;

    public EmailSender(IOptions<EmailSettingNames> options)
    {
        _emailSettingNames = options.Value;
    }

    public async Task SendEmail(string email, string subject, string message)
    {
        var senderMail = _emailSettingNames.Email;
        var pw = _emailSettingNames.Password;

        var client = new SmtpClient(_emailSettingNames.SmtpClient, _emailSettingNames.Port)
        {
            EnableSsl = _emailSettingNames.EnableSsl,
        };
        client.UseDefaultCredentials = _emailSettingNames.UseDefaultCredentials;
        client.Credentials = new NetworkCredential(senderMail, pw);

        
        Email.DefaultSender = new SmtpSender(client);

        //should make use of this
        var gido = await Email
            .From(senderMail).To(email)
            .Subject(subject).Body(message)
            .SendAsync();
           
    }
    public async Task SendHtmlEmail(string email, string subject, string template )
    {
        var senderMail = _emailSettingNames.Email;
        var pw = _emailSettingNames.Password;

        var client = new SmtpClient(_emailSettingNames.SmtpClient, _emailSettingNames.Port)
        {
            EnableSsl = _emailSettingNames.EnableSsl,
        };
        client.UseDefaultCredentials = _emailSettingNames.UseDefaultCredentials;
        client.Credentials = new NetworkCredential(senderMail, pw);

        
        Email.DefaultSender = new SmtpSender(client);

        var gido = await Email
            .From(senderMail).To(email)
            .Subject(subject).Body(template,true)
            .SendAsync();
    }
}