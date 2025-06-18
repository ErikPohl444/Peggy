using Microsoft.Extensions.Diagnostics.HealthChecks;
using Peggy.Services;

namespace Peggy.HealthChecks;

/// <summary>
/// Health check for the PatronageService.
/// </summary>
public class PatronageServiceHealthCheck : IHealthCheck
{
    private readonly IPatronageService _patronageService;
    private readonly ILogger<PatronageServiceHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the PatronageServiceHealthCheck.
    /// </summary>
    /// <param name="patronageService">The patronage service.</param>
    /// <param name="logger">The logger.</param>
    public PatronageServiceHealthCheck(IPatronageService patronageService, ILogger<PatronageServiceHealthCheck> logger)
    {
        _patronageService = patronageService;
        _logger = logger;
    }

    /// <summary>
    /// Checks the health of the PatronageService.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking PatronageService health...");

            // Try to get all patronages as a health check
            var patronages = await _patronageService.GetAllPatronagesAsync();
            
            _logger.LogInformation("PatronageService health check passed");
            return HealthCheckResult.Healthy("PatronageService is healthy", new Dictionary<string, object>
            {
                { "TotalPatronages", patronages.Count() }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PatronageService health check failed");
            return HealthCheckResult.Unhealthy("PatronageService is unhealthy", ex);
        }
    }
} 