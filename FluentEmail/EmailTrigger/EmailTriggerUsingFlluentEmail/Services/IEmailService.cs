using EmailTriggerUsingFlluentEmail.Models;

namespace EmailTriggerUsingFlluentEmail.Services;

public interface IEmailService
{
    Task Send(EmailMetadata emailMetadata);
    Task SendUsingTemplate(EmailMetadata emailMetadata, User user, string templateName);
    Task SendWithAttachment(EmailMetadata emailMetadata);
    Task SendMultiple(List<EmailMetadata> emailMetadata);
}
