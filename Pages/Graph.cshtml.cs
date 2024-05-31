using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TexasGuyContract.Pages.Forms;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Models;

namespace TexasGuyContractIdentity.Pages
{
    public class GraphModel : PageModel
    {

        private readonly ILogger<GraphModel> _logger;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public History HistoryEntry { get; set; } = new History();

        public List<SelectListItem> Stations { get; set; }

        public int tempID = 0;

        public GraphModel(ILogger<GraphModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Populate stations dropdown
                Stations = await _context.StationsEntries
                    .Select(s => new SelectListItem
                    {
                        Value = s.ID.ToString(),
                        Text = s.StationName,
                    })
                    .ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading stations");
                return RedirectToPage("/Error");
            }
        }


    }
}
