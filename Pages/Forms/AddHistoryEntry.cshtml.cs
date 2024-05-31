using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Models;

namespace TexasGuyContract.Pages.Forms
{
    [Authorize]
    public class AddHistoryEntryModel : PageModel
    {
        private readonly ILogger<AddStationModel> _logger;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public History HistoryEntry { get; set; } = new History();

        public List<SelectListItem> Stations { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        public AddHistoryEntryModel(ILogger<AddStationModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (Id.HasValue)
            {
                HistoryEntry = _context.HistoryEntries.FirstOrDefault(h => h.ID == Id);

                if (HistoryEntry == null)
                {
                    return RedirectToPage("/Error");
                }
            }

            PopulateStationsDropdown();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                PopulateStationsDropdown();
                return Page();
            }

            HistoryEntry.Timestamp = DateTime.Now;

            _context.HistoryEntries.Add(HistoryEntry);
            _context.SaveChanges();

            HistoryEntry = new History();

            return RedirectToPage("/History");
        }

        private void PopulateStationsDropdown()
        {
            Stations = _context.StationsEntries
                .Where(s => s.isEnabled)
                .Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.StationName
                })
                .ToList();
        }
    }
}
