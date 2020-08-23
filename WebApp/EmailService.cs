using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApp
{
    public class EmailService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("ocph23.test@gmail.com", "Sony@7777"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ocph23.test@gmail.com", "Pertamina"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                return Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                return Task.CompletedTask;
            }
        }
    }
}