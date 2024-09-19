using EmailTriggerUsingFlluentEmail.Models;
using EmailTriggerUsingFlluentEmail.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailTriggerUsingFlluentEmail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("SendEmail")]
        public async Task<IActionResult> SendEmail()
        {
            EmailMetadata metadata = new EmailMetadata(
                    "recipient@example.com",
               "Test Email",
               "This is a test email.");
            
            await _emailService.Send(metadata);
            return Ok("Email sent successfully");
        }

        [HttpGet("SendEmailWithAttachment")]
        public async Task<IActionResult> SendEmailWithAttachment()
        {
            EmailMetadata metadata = new EmailMetadata(
                   "recipient@example.com",
              "Test Email",
              "This is a test email.",
              $"{Directory.GetCurrentDirectory()}/TestFile.txt");
            await _emailService.SendWithAttachment(metadata);
            return Ok("Email with attachment sent successfully");
        }

        [HttpGet("SendEmailUsingTemplate")]
        public async Task<IActionResult> SendEmailUsingTemplate()
        {
            User user = new User("John Doe",
                "john.doe@example.com",
                "Gold");
           EmailMetadata metadata = new EmailMetadata(
                    user.Email,
               "Test Email with Template",
               "This is a test email with a template.");
            var templateFile = $"{Directory.GetCurrentDirectory()}/EmailTemplate.cshtml";
            await _emailService.SendUsingTemplate(metadata, user, templateFile);
            return Ok("Email using template sent successfully");
        }

        [HttpGet("SendMultipleEmails")]
        public async Task<IActionResult> SendMultipleEmails()
        {
            List<User> users = new List<User>
            {
                new User("John Doe",
                    "john.doe@example.com",
                    "Gold"),
                new User("Jane Smith",
                    "jane.smith@example.com",
                    "Silver")
            };
            List<EmailMetadata> emailMetadatas = new List<EmailMetadata>();
            foreach (var user in users)
            {
                EmailMetadata metadata = new EmailMetadata(
                        user.Email,
                   "Test Email",
                   "This is a test email.");
                emailMetadatas.Add(metadata);
            }
            await _emailService.SendMultiple(emailMetadatas);
            return Ok("Multiple emails sent successfully");
        }

    }
}
