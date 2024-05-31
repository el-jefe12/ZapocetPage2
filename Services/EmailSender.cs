using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Threading.Tasks;
using System;

namespace TexasGuyContractIdentity.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {
        }

        public async Task SendEmailAsync(string receiver, string subject, string body)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("noreply@codeclimber.cz"); // Change to your sender address
                mail.To.Add(receiver);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                try
                {
                    using (var smtp = new SmtpClient("localhost", 25)) // Papercut settings
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = false; // Papercut doesn't support SSL
                        smtp.Timeout = 10000; // Set timeout to 10 seconds
                        await smtp.SendMailAsync(mail);
                    }
                    Console.WriteLine($"Email sent to {receiver}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {receiver}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
