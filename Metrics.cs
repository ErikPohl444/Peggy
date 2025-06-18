using Prometheus;

namespace Peggy;

public static class Metrics
{
    // User metrics
    public static readonly Counter UserCreationCounter = Metrics.CreateCounter(
        "peggy_user_creation_total",
        "Total number of users created"
    );

    public static readonly Counter UserUpdateCounter = Metrics.CreateCounter(
        "peggy_user_update_total",
        "Total number of user updates"
    );

    public static readonly Counter UserDeletionCounter = Metrics.CreateCounter(
        "peggy_user_deletion_total",
        "Total number of users deleted"
    );

    // Project metrics
    public static readonly Counter ProjectCreationCounter = Metrics.CreateCounter(
        "peggy_project_creation_total",
        "Total number of projects created"
    );

    public static readonly Counter ProjectUpdateCounter = Metrics.CreateCounter(
        "peggy_project_update_total",
        "Total number of project updates"
    );

    public static readonly Counter ProjectDeletionCounter = Metrics.CreateCounter(
        "peggy_project_deletion_total",
        "Total number of projects deleted"
    );

    // Patronage metrics
    public static readonly Counter PatronageCreationCounter = Metrics.CreateCounter(
        "peggy_patronage_creation_total",
        "Total number of patronages created"
    );

    public static readonly Counter PatronageUpdateCounter = Metrics.CreateCounter(
        "peggy_patronage_update_total",
        "Total number of patronage updates"
    );

    public static readonly Counter PatronageDeletionCounter = Metrics.CreateCounter(
        "peggy_patronage_deletion_total",
        "Total number of patronages deleted"
    );

    // Patronage Payment metrics
    public static readonly Counter PaymentCreationCounter = Metrics.CreateCounter(
        "peggy_payment_creation_total",
        "Total number of payments created"
    );

    public static readonly Counter PaymentUpdateCounter = Metrics.CreateCounter(
        "peggy_payment_update_total",
        "Total number of payment updates"
    );

    public static readonly Counter PaymentDeletionCounter = Metrics.CreateCounter(
        "peggy_payment_deletion_total",
        "Total number of payments deleted"
    );

    // API metrics
    public static readonly Histogram ApiRequestDuration = Metrics.CreateHistogram(
        "peggy_api_request_duration_seconds",
        "API request duration in seconds",
        new HistogramConfiguration
        {
            Buckets = new[] { 0.1, 0.5, 1, 2, 5 }
        }
    );

    public static readonly Counter ApiRequestCounter = Metrics.CreateCounter(
        "peggy_api_requests_total",
        "Total number of API requests",
        new CounterConfiguration
        {
            LabelNames = new[] { "endpoint", "method", "status_code" }
        }
    );
} 