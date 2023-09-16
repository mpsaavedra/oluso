using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Oluso.HealthCheck.Checkers;

/// <summary>
/// Hosted service to check system health
/// </summary>
public class StartupHostedServiceHealthCheck : IHealthCheck
{
    private volatile bool _startupTaskCompleted = false;

    /// <summary>
    /// Check service name
    /// </summary>
    public string Name = "slow_dependency_check";

    /// <summary>
    /// return startup task
    /// </summary>
    public bool StartupTaskCompleted
    {
        get => _startupTaskCompleted;
        set => _startupTaskCompleted = value;
    }

    /// <summary>
    /// check for startup status
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken()) =>
        StartupTaskCompleted
            ? Task.FromResult(HealthCheckResult.Healthy("Startup task has finished"))
            : Task.FromResult(HealthCheckResult.Unhealthy("Startup task is still running"));
}