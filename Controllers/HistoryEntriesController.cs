using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Models;

[Route("api/[controller]")]
[ApiController]
public class HistoryEntriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HistoryEntriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetHistoryEntries(int? StationID)
    {
        if (StationID == null)
        {
            return BadRequest("StationID is required");
        }

        if (StationID == 0) // Assuming 0 is used for 'All Stations'
        {
            var allData = await _context.HistoryEntries
                .Include(h => h.Station)
                .ToListAsync();

            var groupedData = allData.GroupBy(h => h.StationID)
                                     .Select(g => new
                                     {
                                         StationName = g.First().Station.StationName,
                                         Entries = g.Select(e => new
                                         {
                                             e.Timestamp,
                                             e.Value
                                         })
                                     }).ToList();

            return Ok(groupedData);
        }
        else
        {
            var data = await _context.HistoryEntries
                .Where(h => h.StationID == StationID)
                .Select(h => new
                {
                    h.Timestamp,
                    h.Value
                }).ToListAsync();

            return Ok(data);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddHistoryEntry([FromBody] History historyEntry)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        historyEntry.Timestamp = DateTime.Now;
        _context.HistoryEntries.Add(historyEntry);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHistoryEntries), new { StationID = historyEntry.StationID }, historyEntry);
    }
}
