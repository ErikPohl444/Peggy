using Microsoft.Extensions.Diagnostics.HealthChecks;
using Peggy.Services;

namespace Peggy.HealthChecks;

/// <summary>
/// Health check for the ProjectService.
/// </summary>
public class ProjectServiceHealthCheck : IHealthCheck
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectServiceHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the ProjectServiceHealthCheck.
    /// </summary>
    /// <param name="projectService">The project service.</param>
    /// <param name="logger">The logger.</param>
    public ProjectServiceHealthCheck(IProjectService projectService, ILogger<ProjectServiceHealthCheck> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Checks the health of the ProjectService.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking ProjectService health...");

            // Try to get all projects as a health check
            var projects = await _projectService.GetAllProjectsAsync();
            
            _logger.LogInformation("ProjectService health check passed");
            return HealthCheckResult.Healthy("ProjectService is healthy", new Dictionary<string, object>
            {
                { "TotalProjects", projects.Count() }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProjectService health check failed");
            return HealthCheckResult.Unhealthy("ProjectService is unhealthy", ex);
        }
    }
} 