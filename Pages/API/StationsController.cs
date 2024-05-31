using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Models;

namespace TexasGuyContract.Pages.API
{
    [Route("api")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-stations")]
        public IActionResult GetListOfStations()
        {
            var list = _context.StationsEntries.ToList();
            return StatusCode(200, new JsonResult(list));

        }

        [HttpPost]
        [Route("add-stations")]
        public IActionResult AddStation(Stations station)
        {
            if (_context.StationsEntries.Where(x => x.StationName.Equals(station.StationName)).Any())
            {
                return StatusCode(400,
                    new JsonResult("Duplicities are not allowed. Sorry."));
            }

            _context.StationsEntries.Add(station);
            _context.SaveChanges();

            return StatusCode(200,
                new JsonResult("Station has been successfully added."));
        }

        [HttpPost("UpdateStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, bool isEnabled)
        {
            try
            {
                var station = await _context.StationsEntries.FindAsync(id);

                if (station == null)
                {
                    return NotFound(new { Message = "Station not found" });
                }

                station.isEnabled = isEnabled;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Station status updated successfully",
                    StationId = id,
                    IsEnabled = isEnabled
                });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details for debugging
                //logger.LogError($"Failed to update station status. Error: {ex.Message}");
                return BadRequest(new { Message = "Failed to update station status. Database error." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Failed to update station status. Error: {ex.Message}" });
            }
        }
    }
}
