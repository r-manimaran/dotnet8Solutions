namespace EmailTriggerUsingFlluentEmail.Models
{
    public class EmailMetadata
    {
        public EmailMetadata(string toEmail, string subject, string? body="", string? attachmentPath="")
        {
            ToEmail = toEmail;
            Subject = subject;
            Body = body;
            AttachmentPath = attachmentPath;
        }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public string? AttachmentPath { get; set; } // Optional attachment path
    }
}
