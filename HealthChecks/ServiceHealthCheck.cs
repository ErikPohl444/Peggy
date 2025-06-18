using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Text.Json;

namespace Peggy.HealthChecks;

/// <summary>
/// Health check for external service dependencies.
/// </summary>
public class ServiceHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ServiceHealthCheck> _logger;
    private readonly string _serviceName;
    private readonly string _serviceUrl;

    /// <summary>
    /// Initializes a new instance of the ServiceHealthCheck.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="serviceName">The name of the service to check.</param>
    /// <param name="serviceUrl">The URL of the service to check.</param>
    public ServiceHealthCheck(
        HttpClient httpClient,
        ILogger<ServiceHealthCheck> logger,
        string serviceName,
        string serviceUrl)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serviceName = serviceName;
        _serviceUrl = serviceUrl;
    }

    /// <summary>
    /// Checks the health of the external service.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking health of service {ServiceName} at {ServiceUrl}", 
                _serviceName, _serviceUrl);

            var response = await _httpClient.GetAsync(_serviceUrl, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Service {ServiceName} is healthy", _serviceName);
                return HealthCheckResult.Healthy($"Service {_serviceName} is healthy");
            }

            _logger.LogWarning("Service {ServiceName} returned status code {StatusCode}", 
                _serviceName, response.StatusCode);
            return HealthCheckResult.Unhealthy($"Service {_serviceName} returned status code {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed for service {ServiceName}", _serviceName);
            return HealthCheckResult.Unhealthy($"Service {_serviceName} is unhealthy", ex);
        }
    }
} 