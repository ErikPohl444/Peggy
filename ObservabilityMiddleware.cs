using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Peggy;

public class ObservabilityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ObservabilityMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var path = context.Request.Path.Value ?? "";
        var method = context.Request.Method;

        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            var statusCode = context.Response.StatusCode;

            // Log the request
            _logger.Information(
                "Request {Method} {Path} completed with status {StatusCode} in {ElapsedMilliseconds}ms",
                method,
                path,
                statusCode,
                sw.ElapsedMilliseconds
            );

            // Record metrics
            Metrics.ApiRequestDuration.Observe(sw.Elapsed.TotalSeconds);
            Metrics.ApiRequestCounter
                .WithLabels(path, method, statusCode.ToString())
                .Inc();
        }
    }
} 