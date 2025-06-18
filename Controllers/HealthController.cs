using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text;

namespace Peggy.Controllers;

/// <summary>
/// Controller for health check endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;
    private readonly ILogger<HealthController> _logger;

    /// <summary>
    /// Initializes a new instance of the HealthController.
    /// </summary>
    /// <param name="healthCheckService">The health check service.</param>
    /// <param name="logger">The logger.</param>
    public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
    {
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    /// <summary>
    /// Gets the health status of all services.
    /// </summary>
    /// <returns>The health status report.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthReport), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HealthReport), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync();
        _logger.LogInformation("Health check completed with status: {Status}", report.Status);

        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode(503, report);
    }

    /// <summary>
    /// Gets the health status of a specific service.
    /// </summary>
    /// <param name="service">The name of the service to check.</param>
    /// <returns>The health status of the specified service.</returns>
    [HttpGet("{service}")]
    [ProducesResponseType(typeof(HealthReport), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HealthReport), StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceHealth(string service)
    {
        var report = await _healthCheckService.CheckHealthAsync(reg => reg.Tags.Contains(service));
        
        if (report.Entries.Count == 0)
        {
            return NotFound($"Service '{service}' not found");
        }

        _logger.LogInformation("Health check for service {Service} completed with status: {Status}", 
            service, report.Status);

        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode(503, report);
    }
} 