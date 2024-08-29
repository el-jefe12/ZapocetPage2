using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Threading.Tasks;
using System;
using TexasGuyContractIdentity.Data;
using Microsoft.EntityFrameworkCore;

namespace TexasGuyContractIdentity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationDbContext _context;

        public EmailSender(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendEmailAsync(string receiver, string subject, string body)
        {
            // Fetch the active SMTP configuration
            var smtpConfig = await _context.SmtpModels.FirstOrDefaultAsync(s => s.is_active);
            if (smtpConfig == null)
            {
                throw new InvalidOperationException("No active SMTP configuration found.");
            }

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("noreply@texasguycontract.com"); // Set your sender address
                mail.To.Add(receiver);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                try
                {
                    using (var smtp = new SmtpClient("localhost", smtpConfig.Port))
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = false; // Modify this if your SMTP requires SSL
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
