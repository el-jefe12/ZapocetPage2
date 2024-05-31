using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Generic;
using TexasGuyContractIdentity.Models;

namespace TexasGuyContract.Pages.Forms
{
    [Authorize]
    public class AddStationModel : GenericPageModel
    {
        private readonly ILogger<AddStationModel> _logger;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Stations Station { get; set; } = new Stations();

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        public AddStationModel(ILogger<AddStationModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (Id.HasValue)
            {
                // If Id is provided, retrieve the station details from the database for editing
                Station = _context.StationsEntries.FirstOrDefault(s => s.ID == Id);

                if (Station == null)
                {
                    // Handle not found scenario, redirect to a proper page or return an error
                    return RedirectToPage("/Error");
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set default email if not provided
            if (string.IsNullOrEmpty(Station.Email))
            {
                Station.Email = "No email";
            }

            if (Id.HasValue)
            {
                // Edit existing station logic
                var existingStation = _context.StationsEntries.Find(Id);
                if (existingStation != null)
                {
                    existingStation.StationName = Station.StationName;
                    existingStation.Minutes = Station.Minutes;
                    existingStation.LowestLevel = Station.LowestLevel;
                    existingStation.HighestLevel = Station.HighestLevel;
                    existingStation.Email = Station.Email;
                    //existingStation.isEnabled = Station.isEnabled;
                }
            }
            else
            {
                // Add new station logic
                _context.StationsEntries.Add(Station);
            }

            _context.SaveChanges();

            // Reset the Station property for a new entry
            Station = new Stations();

            ModelState.Clear();

            return RedirectToPage("/Stations");
        }
    }
}
