using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Peggy.Data;
using Peggy.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using OpenTelemetry.Trace;

namespace Peggy.Services
{
    public interface IPatronageService
    {
        Task<Patronage> CreatePatronageAsync(Patronage patronage);
        Task<Patronage?> GetPatronageAsync(int id);
        Task<IEnumerable<Patronage>> GetAllPatronagesAsync();
        Task<Patronage> UpdatePatronageAsync(Patronage patronage);
        Task DeletePatronageAsync(int id);
    }

    public class PatronageService : IPatronageService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PatronageService> _logger;
        private static readonly ActivitySource ActivitySource = new("Peggy.PatronageService");

        public PatronageService(AppDbContext context, ILogger<PatronageService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Patronage> CreatePatronageAsync(Patronage patronage)
        {
            using var activity = ActivitySource.StartActivity("CreatePatronage");
            activity?.SetTag("patronage.user_id", patronage.UserId);
            activity?.SetTag("patronage.project_id", patronage.ProjectId);

            try
            {
                _context.Patronages.Add(patronage);
                await _context.SaveChangesAsync();
                
                Metrics.PatronageCreationCounter.Inc();
                _logger.LogInformation("Created patronage with ID {PatronageId}", patronage.Id);
                
                return patronage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patronage");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<Patronage?> GetPatronageAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("GetPatronage");
            activity?.SetTag("patronage.id", id);

            try
            {
                var patronage = await _context.Patronages.FindAsync(id);
                
                if (patronage == null)
                {
                    _logger.LogWarning("Patronage with ID {PatronageId} not found", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Patronage not found");
                }
                else
                {
                    _logger.LogInformation("Retrieved patronage with ID {PatronageId}", id);
                }
                
                return patronage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patronage with ID {PatronageId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Patronage>> GetAllPatronagesAsync()
        {
            using var activity = ActivitySource.StartActivity("GetAllPatronages");

            try
            {
                var patronages = await _context.Patronages.ToListAsync();
                _logger.LogInformation("Retrieved {Count} patronages", patronages.Count);
                return patronages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patronages");
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task<Patronage> UpdatePatronageAsync(Patronage patronage)
        {
            using var activity = ActivitySource.StartActivity("UpdatePatronage");
            activity?.SetTag("patronage.id", patronage.Id);

            try
            {
                _context.Entry(patronage).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                Metrics.PatronageUpdateCounter.Inc();
                _logger.LogInformation("Updated patronage with ID {PatronageId}", patronage.Id);
                
                return patronage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patronage with ID {PatronageId}", patronage.Id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }

        public async Task DeletePatronageAsync(int id)
        {
            using var activity = ActivitySource.StartActivity("DeletePatronage");
            activity?.SetTag("patronage.id", id);

            try
            {
                var patronage = await _context.Patronages.FindAsync(id);
                if (patronage == null)
                {
                    _logger.LogWarning("Patronage with ID {PatronageId} not found for deletion", id);
                    activity?.SetStatus(ActivityStatusCode.Error, "Patronage not found");
                    return;
                }

                _context.Patronages.Remove(patronage);
                await _context.SaveChangesAsync();
                
                Metrics.PatronageDeletionCounter.Inc();
                _logger.LogInformation("Deleted patronage with ID {PatronageId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patronage with ID {PatronageId}", id);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
        }
    }
} 