using Microsoft.AspNetCore.Identity.UI.Services;

namespace BulkyWeb.Utility
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email sending logic here
            return Task.CompletedTask;
        }
    }
}
