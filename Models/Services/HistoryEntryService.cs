using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TexasGuyContractIdentity.Data;

namespace TexasGuyContractIdentity.Models.Services
{
    public class HistoryEntryService
    {
        private readonly ApplicationDbContext _context;

        public HistoryEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddHistoryEntryAsync(History entry)
        {
            _context.HistoryEntries.Add(entry);
            await _context.SaveChangesAsync();
            await UpdateStationLevelsAsync(entry.StationID);
        }

        public async Task UpdateHistoryEntryAsync(History entry)
        {
            _context.HistoryEntries.Update(entry);
            await _context.SaveChangesAsync();
            await UpdateStationLevelsAsync(entry.StationID);
        }

        public async Task UpdateStationLevelsAsync(int stationId)
        {
            try
            {
                var command = "EXEC UpdateStationLevels @StationID";
                var parameter = new SqlParameter("@StationID", stationId);
                await _context.Database.ExecuteSqlRawAsync(command, parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
        }

        // Scheduled task to update all stations
        public async Task UpdateAllStationsAsync()
        {
            var stations = await _context.StationsEntries.ToListAsync();
            foreach (var station in stations)
            {
                await UpdateStationLevelsAsync(station.ID);
            }
        }
    }
}
