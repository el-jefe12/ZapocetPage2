using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TexasGuyContractIdentity.Models;
using TexasGuyContractIdentity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TexasGuyContract.Pages
{
    [Authorize]
    public class StationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Stations> StationList { get; set; } = new List<Stations>();

        public StationsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            StationList = _context.StationsEntries.ToList();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, bool isEnabled)
        {
            var station = await _context.StationsEntries.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            station.isEnabled = isEnabled;
            await _context.SaveChangesAsync();

            return RedirectToPage(); // Redirect to the same page to refresh the data
        }
    }
}
