using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TexasGuyContractIdentity.Data;

namespace TexasGuyContractIdentity.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConfigModel> _logger;

        public ConfigModel(ApplicationDbContext context, ILogger<ConfigModel> logger, IEmailSender emailSender)
        {
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public EmailFormModel EmailForm { get; set; }

        [BindProperty]
        public SmtpFormModel SmtpForm { get; set; }

        [BindProperty]
        public SmtpSelectionModel SmtpSelection { get; set; }

        public List<SmtpModel> SmtpList { get; set; }

        public async Task OnGetAsync()
        {
            SmtpList = await _context.SmtpModels.ToListAsync();
        }

        public async Task<IActionResult> OnPostSendTestMailAsync()
        {
            ModelState.Clear(); // Clear any previous validation errors.
            if (!TryValidateModel(EmailForm, nameof(EmailForm)))
            {
                _logger.LogError("Email form state is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return Page();
            }

            await _emailSender.SendEmailAsync(EmailForm.Email, EmailForm.Subject, EmailForm.Body);
            TempData["Message"] = "Email sent successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddSmtpAsync()
        {
            ModelState.Clear(); // Clear any previous validation errors.
            if (!TryValidateModel(SmtpForm, nameof(SmtpForm)))
            {
                _logger.LogError("SMTP form state is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return Page();
            }

            var smtp = new SmtpModel
            {
                Name = SmtpForm.Name,
                Port = SmtpForm.Port,
                is_active = false
            };

            _context.SmtpModels.Add(smtp);
            await _context.SaveChangesAsync();

            TempData["Message"] = "SMTP added successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSelectSmtpAsync()
        {
            ModelState.Clear(); // Clear any previous validation errors.
            if (!TryValidateModel(SmtpSelection, nameof(SmtpSelection)))
            {
                _logger.LogError("SMTP selection form state is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return Page();
            }

            var smtps = await _context.SmtpModels.ToListAsync();

            foreach (var smtp in smtps)
            {
                smtp.is_active = smtp.Id == SmtpSelection.SelectedSmtpId;
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "SMTP selected successfully!";
            return RedirectToPage();
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

        public class SmtpFormModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public int Port { get; set; }
        }

        public class SmtpSelectionModel
        {
            [Required]
            public int SelectedSmtpId { get; set; }
        }

        public class SmtpModel
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public int Port { get; set; }

            public bool is_active { get; set; }
        }
    }
}
