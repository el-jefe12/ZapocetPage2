using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TexasGuyContractIdentity.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        public ConfigModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [BindProperty]
        public EmailFormModel EmailForm { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _emailSender.SendEmailAsync(EmailForm.Email, EmailForm.Subject, EmailForm.Body);
                TempData["Message"] = "Email sent successfully!";
                return RedirectToPage();
            }

            return Page();
        }

        public class EmailFormModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Subject { get; set; }

            [Required]
            public string Body { get; set; }
        }
    }
}
