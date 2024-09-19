using EmailTriggerUsingFlluentEmail.Models;
using FluentEmail.Core;

namespace EmailTriggerUsingFlluentEmail.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailService(IFluentEmail fluentEmail, IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmail = fluentEmail;
        _fluentEmailFactory = fluentEmailFactory;
    }

    public async Task Send(EmailMetadata emailMetadata)
    {
       await _fluentEmail.To(emailMetadata.ToEmail)
            .Subject(emailMetadata.Subject)
            .Body(emailMetadata.Body)
            .SendAsync();
    }

    public async Task SendMultiple(List<EmailMetadata> emailMetadata)
    {
        foreach (var email in emailMetadata)
        {
          await _fluentEmailFactory.Create()
                .To(email.ToEmail)
                .Subject(email.Subject)
                .Body(email.Body)
                .SendAsync();
        }
    }

    public async Task SendUsingTemplate(EmailMetadata emailMetadata, User user, string templateName)
    {
       await _fluentEmail.To(emailMetadata.ToEmail)
            .Subject(emailMetadata.Subject)
            .UsingTemplateFromFile(templateName, user)
            .SendAsync();
    }

    public async Task SendWithAttachment(EmailMetadata emailMetadata)
    {
        await _fluentEmail.To(emailMetadata.ToEmail)
            .Subject(emailMetadata.Subject)
            .Body(emailMetadata.Body)
            .AttachFromFilename(emailMetadata.AttachmentPath,
            attachmentName:Path.GetFileName(emailMetadata.AttachmentPath) )          
            .SendAsync();
    }
}

