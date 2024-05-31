using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TexasGuyContractIdentity.Generic;
using TexasGuyContractIdentity.Models;
using TexasGuyContractIdentity.Data;
using Microsoft.AspNetCore.Authorization;

namespace TexasGuyContract.Pages
{
    [Authorize]
    public class HistoryModel : GenericPageModel
    {
        public List<History> HistoryList { get; set; } = new List<History>();

        public HistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            HistoryList = await _context.HistoryEntries.Include(d => d.Station).ToListAsync();
            return Page();
        }
    }
}
