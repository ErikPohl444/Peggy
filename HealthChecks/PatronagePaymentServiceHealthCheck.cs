using Microsoft.Extensions.Diagnostics.HealthChecks;
using Peggy.Services;

namespace Peggy.HealthChecks;

/// <summary>
/// Health check for the PatronagePaymentService.
/// </summary>
public class PatronagePaymentServiceHealthCheck : IHealthCheck
{
    private readonly IPatronagePaymentService _paymentService;
    private readonly ILogger<PatronagePaymentServiceHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the PatronagePaymentServiceHealthCheck.
    /// </summary>
    /// <param name="paymentService">The payment service.</param>
    /// <param name="logger">The logger.</param>
    public PatronagePaymentServiceHealthCheck(IPatronagePaymentService paymentService, ILogger<PatronagePaymentServiceHealthCheck> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Checks the health of the PatronagePaymentService.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking PatronagePaymentService health...");

            // Try to get all payments as a health check
            var payments = await _paymentService.GetAllPaymentsAsync();
            
            _logger.LogInformation("PatronagePaymentService health check passed");
            return HealthCheckResult.Healthy("PatronagePaymentService is healthy", new Dictionary<string, object>
            {
                { "TotalPayments", payments.Count() }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PatronagePaymentService health check failed");
            return HealthCheckResult.Unhealthy("PatronagePaymentService is unhealthy", ex);
        }
    }
} 