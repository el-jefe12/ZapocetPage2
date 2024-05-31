using Microsoft.AspNetCore.Identity.UI.Services;

namespace TexasGuyContractIdentity.Services
{
    public class NotificationService
    {
        readonly IEmailSender _emailSender;

        public NotificationService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void Send(string subject, string content, string recievers)
        {
            foreach (var recipient in recievers.Split(';'))
            {
                _emailSender.SendEmailAsync(recipient, content, subject);
            }
        }
    }
}
