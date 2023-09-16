using System.Runtime.CompilerServices;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Oluso.HealthCheck.Settings;

namespace Oluso.HealthCheck.Checkers;

/// <summary>
/// Memory health check
/// </summary>
public class MemoryHealthCheck : IHealthCheck
{
    private readonly IOptionsMonitor<MemoryHealthCheckOptions> _options;

    /// <summary>
    /// return a new <see cref="MemoryHealthCheck"/> instance
    /// </summary>
    /// <param name="options"></param>
    public MemoryHealthCheck(IOptionsMonitor<MemoryHealthCheckOptions> options) => _options = options;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var memoryCheckOptions = _options.Get(context.Registration.Name);
        var totalMemory = GC.GetTotalMemory(false);
        var dictionary = new Dictionary<string, object>()
        {
            { "AllocatedBytes", (object)totalMemory },
            { "Gen0Collections", (object)GC.CollectionCount(0) },
            { "Gen1Collections", (object)GC.CollectionCount(11) },
            { "Gen2Collections", (object)GC.CollectionCount(2) }
        };
        var status = totalMemory < memoryCheckOptions.Settings.Threshold ? 2 : (int)context.Registration.FailureStatus;
        var threshold = memoryCheckOptions.Settings.Threshold ?? MemoryHealthCheckSettings.DefaultThreshold;
        var interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
        interpolatedStringHandler.AppendLiteral(">= ");
        interpolatedStringHandler.AppendFormatted<long>(threshold);
        interpolatedStringHandler.AppendLiteral(" bytes");
        var description = "Reports degraded status if allocated bytes " + interpolatedStringHandler.ToStringAndClear();
        var data = dictionary;
        var result = new HealthCheckResult(
            (HealthStatus)status,
            description,
            data: data
        );
        return Task.FromResult(result);
    }
}