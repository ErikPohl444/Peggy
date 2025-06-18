# Observability in Peggy

This document describes the observability features implemented in the Peggy system.

## Overview

Peggy implements a comprehensive observability stack that includes:

1. **Logging**: Using Serilog for structured logging
2. **Metrics**: Using Prometheus for metrics collection
3. **Tracing**: Using OpenTelemetry for distributed tracing

## Logging

### Configuration

Logging is configured in `appsettings.json` and uses Serilog with the following sinks:
- Console output for development
- File output with daily rolling logs

### Log Levels

- Default: Information
- Microsoft: Warning
- System: Warning

### Log Format

Console logs:
```
[HH:mm:ss LEVEL] Message
Exception details (if any)
```

File logs:
```
YYYY-MM-DD HH:mm:ss.fff zzz [LEVEL] Message
Exception details (if any)
```

## Metrics

### Available Metrics

#### User Metrics
- `peggy_user_creation_total`: Counter for user creation
- `peggy_user_update_total`: Counter for user updates
- `peggy_user_deletion_total`: Counter for user deletion

#### Project Metrics
- `peggy_project_creation_total`: Counter for project creation
- `peggy_project_update_total`: Counter for project updates
- `peggy_project_deletion_total`: Counter for project deletion

#### Patronage Metrics
- `peggy_patronage_creation_total`: Counter for patronage creation
- `peggy_patronage_update_total`: Counter for patronage updates
- `peggy_patronage_deletion_total`: Counter for patronage deletion

#### Payment Metrics
- `peggy_payment_creation_total`: Counter for payment creation
- `peggy_payment_update_total`: Counter for payment updates
- `peggy_payment_deletion_total`: Counter for payment deletion

#### API Metrics
- `peggy_api_request_duration_seconds`: Histogram of API request durations
- `peggy_api_requests_total`: Counter of API requests with labels for endpoint, method, and status code

### Accessing Metrics

Metrics are exposed on port 9090 at the `/metrics` endpoint.

## Tracing

### Configuration

Tracing is configured using OpenTelemetry with:
- ASP.NET Core instrumentation
- HTTP client instrumentation
- Console exporter for development

### Available Traces

The following operations are traced:
- User operations (create, read, update, delete)
- Project operations
- Patronage operations
- Payment operations
- API requests

### Trace Attributes

Each trace includes:
- Operation name
- Start and end timestamps
- Duration
- Status (success/error)
- Relevant tags (e.g., user ID, project ID)

## Monitoring

### Prometheus

To monitor the system using Prometheus:

1. Install Prometheus
2. Configure Prometheus to scrape metrics from `http://localhost:9090/metrics`
3. Set up appropriate alerting rules

### Grafana

For visualization:

1. Install Grafana
2. Configure Prometheus as a data source
3. Create dashboards for:
   - API performance
   - User operations
   - Project operations
   - Patronage operations
   - Payment operations

## Best Practices

1. **Logging**
   - Use structured logging with appropriate log levels
   - Include relevant context in log messages
   - Don't log sensitive information

2. **Metrics**
   - Use appropriate metric types (counter, gauge, histogram)
   - Include relevant labels
   - Set appropriate bucket sizes for histograms

3. **Tracing**
   - Keep trace names consistent
   - Include relevant tags
   - Set appropriate sampling rates

## Troubleshooting

### Common Issues

1. **Missing Logs**
   - Check log file permissions
   - Verify log level configuration
   - Check disk space

2. **Missing Metrics**
   - Verify Prometheus configuration
   - Check metric endpoint accessibility
   - Verify metric names

3. **Missing Traces**
   - Check OpenTelemetry configuration
   - Verify trace sampling configuration
   - Check trace exporter configuration 