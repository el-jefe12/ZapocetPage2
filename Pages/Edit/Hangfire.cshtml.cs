using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TexasGuyContractIdentity.Services;

namespace TexasGuyContractIdentity.Pages.Edit
{
    public class HangfireModel : PageModel
    {
        public void OnGet()
        {
            Hangfire.BackgroundJob.Enqueue<EmailSender>(x => x.SendEmailAsync("steveklein@seznam.cz", "body", "body"));

        }
    }
}
