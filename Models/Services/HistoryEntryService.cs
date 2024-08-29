using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TexasGuyContractIdentity.Data;

namespace TexasGuyContractIdentity.Models.Services
{
    public class HistoryEntryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HistoryEntryService> _logger;

        public HistoryEntryService(ApplicationDbContext context, ILogger<HistoryEntryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddHistoryEntryAsync(History entry)
        {
            try
            {
                entry.Timestamp = DateTime.Now; // Ensure the timestamp is set to current time
                _context.HistoryEntries.Add(entry);
                await _context.SaveChangesAsync();
                await UpdateStationLevelsAsync(entry.StationID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding history entry: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateHistoryEntryAsync(History entry)
        {
            try
            {
                _context.HistoryEntries.Update(entry);
                await _context.SaveChangesAsync();
                await UpdateStationLevelsAsync(entry.StationID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating history entry: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateStationLevelsAsync(int stationId)
        {
            try
            {
                _logger.LogInformation($"Updating station levels for StationID: {stationId}");

                var historyEntries = await _context.HistoryEntries
                    .Where(h => h.StationID == stationId)
                    .ToListAsync();

                if (historyEntries.Any())
                {
                    var minWaterLevel = historyEntries.Min(h => h.Value);
                    var maxWaterLevel = historyEntries.Max(h => h.Value);

                    var station = await _context.StationsEntries.FindAsync(stationId);
                    if (station != null)
                    {
                        station.LowestLevel = minWaterLevel;
                        station.HighestLevel = maxWaterLevel;

                        _context.StationsEntries.Update(station);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation($"Successfully updated station levels for StationID: {stationId}");
                    }
                    else
                    {
                        _logger.LogWarning($"Station not found for StationID: {stationId}");
                    }
                }
                else
                {
                    _logger.LogWarning($"No history entries found for StationID: {stationId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating station levels: {ex.Message}");
                throw;
            }
        }

        // Scheduled task to update all stations
        public async Task UpdateAllStationsAsync()
        {
            try
            {
                var stations = await _context.StationsEntries.ToListAsync();
                foreach (var station in stations)
                {
                    await UpdateStationLevelsAsync(station.ID);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating all stations: {ex.Message}");
                throw;
            }
        }
    }
}
