using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TexasGuyContractIdentity.Models;
using TexasGuyContractIdentity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TexasGuyContract.Pages
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<History> HistoryList { get; set; } = new List<History>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";
        public string SearchTerm { get; set; }

        public HistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, int itemsPerPage = 10, string sortBy = "ID", string sortOrder = "asc", string searchTerm = "")
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            SortBy = sortBy;
            SortOrder = sortOrder;
            SearchTerm = searchTerm;

            var query = _context.HistoryEntries.Include(d => d.Station).AsQueryable();

            // Apply search
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(x => x.Station.StationName.Contains(SearchTerm) || x.Value.ToString().Contains(SearchTerm));
            }

            // Apply sorting
            query = SortBy switch
            {
                "StationName" => SortOrder == "asc" ? query.OrderBy(x => x.Station.StationName) : query.OrderByDescending(x => x.Station.StationName),
                "Value" => SortOrder == "asc" ? query.OrderBy(x => x.Value) : query.OrderByDescending(x => x.Value),
                "Timestamp" => SortOrder == "asc" ? query.OrderBy(x => x.Timestamp) : query.OrderByDescending(x => x.Timestamp),
                "Status" => SortOrder == "asc" ? query.OrderBy(x => x.Value) : query.OrderByDescending(x => x.Value),
                _ => SortOrder == "asc" ? query.OrderBy(x => x.ID) : query.OrderByDescending(x => x.ID),
            };

            TotalItems = await query.CountAsync();

            // Apply pagination
            HistoryList = await query.Skip((CurrentPage - 1) * ItemsPerPage)
                                     .Take(ItemsPerPage)
                                     .ToListAsync();

            TotalPages = (int)Math.Ceiling(TotalItems / (double)ItemsPerPage);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Partial("_HistoryPartial", this);
            }

            return Page();
        }

    }
}