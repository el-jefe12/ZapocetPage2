using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace TexasGuyContractIdentity.Pages.Edit
{
    public class SendMailTestModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        public SendMailTestModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await SendEmailTo("STEVEKLEIN@seznam.cz");
            return Page();
        }

        public async Task SendEmailTo(string receiver)
        {
            string subject = "Email 4 you :)";
            string body = "hello";

            // Zavolejte metodu SendEmailAsync z injektovaného IEmailSender
            await _emailSender.SendEmailAsync(receiver, subject, body);
        }
    }
}
