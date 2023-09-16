using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oluso.HealthCheck.Checkers;
using Oluso.HealthCheck.Settings;

namespace Oluso.HealthCheck.Extensions;

/// <summary>
/// Health checks builder related extensions
/// </summary>
public static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Add Memory health check service to the DI container.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    /// <param name="failureStatus"></param>
    /// <param name="tags"></param>
    /// <param name="thresholdInBytes"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMemoryHealthCheck(
        this IHealthChecksBuilder builder,
        string name,
        HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = null,
        long? thresholdInBytes = null)

    {
        builder.AddCheck<MemoryHealthCheck>(
            name, 
            failureStatus ?? (HealthStatus?)1, 
            tags!);
        if (thresholdInBytes.HasValue)
            builder.Services.Configure<MemoryHealthCheckOptions>(name,
                (Action<MemoryHealthCheckOptions>)(options => 
                    options.Settings.Threshold = thresholdInBytes.Value));
        return builder;
    }
}