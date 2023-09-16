using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Oluso.HealthCheck;

/// <summary>
/// publish the application health status
/// </summary>
public class ReadinessPublisher : IHealthCheckPublisher
{
    private readonly ILogger<ReadinessPublisher> _logger;

    /// <summary>
    /// returns a new <see cref="ReadinessPublisher"/> instance
    /// </summary>
    /// <param name="logger"></param>
    public ReadinessPublisher(ILogger<ReadinessPublisher> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// publish status
    /// </summary>
    /// <param name="report"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        if (report.Status == HealthStatus.Healthy)
        {
            _logger.LogInformation("{Timestamp} Readiness Probe Status: {Result}", 
                DateTime.UtcNow, report.Status);
        }
        else
        {
            _logger.LogError("{Timestamp} Readiness Probe Status: {Result}", 
                DateTime.UtcNow, report.Status);
        }

        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}