namespace Oluso.HealthCheck.Settings;

/// <summary>
/// Health check options
/// </summary>
public class HealthCheckOptions
{
    /// <summary>
    /// if true the health check is active
    /// </summary>
    public bool IsHealthCheckEnabled { get; set; }
    
    /// <summary>
    /// configure the Memory health check
    /// </summary>
    public Action<MemoryHealthCheckOptions> MemoryHealthCheckOptions { get; private set; } = x => { };

    /// <summary>
    /// configure Memory health checker
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public HealthCheckOptions UseMemoryHealthCheck(Action<MemoryHealthCheckOptions> options)
    {
        MemoryHealthCheckOptions = options;
        IsHealthCheckEnabled = true;
        return this;
    }
}