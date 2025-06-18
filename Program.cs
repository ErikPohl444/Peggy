using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using Peggy.Data;
using Peggy.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource("Peggy")
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Peggy"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter());

// Configure Prometheus metrics
builder.Services.AddMetricServer(options =>
{
    options.Port = 9090;
});

// Add database context
builder.Services.AddDbContext<PeggyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IPatronageService, PatronageService>();
builder.Services.AddScoped<IPatronagePaymentService, PatronagePaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Add observability middleware
app.UseMiddleware<ObservabilityMiddleware>();

// Enable Prometheus metrics endpoint
app.UseMetricServer();
app.UseHttpMetrics();

app.MapControllers();

app.Run();
