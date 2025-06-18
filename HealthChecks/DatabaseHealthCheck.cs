using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Peggy.Data;

namespace Peggy.HealthChecks;

/// <summary>
/// Health check for the database connection.
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the DatabaseHealthCheck.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public DatabaseHealthCheck(AppDbContext dbContext, ILogger<DatabaseHealthCheck> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Checks the health of the database connection.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking database health...");
            
            // Try to execute a simple query
            await _dbContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
            
            _logger.LogInformation("Database health check passed");
            return HealthCheckResult.Healthy("Database is healthy");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return HealthCheckResult.Unhealthy("Database is unhealthy", ex);
        }
    }
} 