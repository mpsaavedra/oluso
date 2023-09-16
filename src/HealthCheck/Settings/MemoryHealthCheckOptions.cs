namespace Oluso.HealthCheck.Settings;

/// <summary>
/// Memory health check options
/// </summary>
public class MemoryHealthCheckOptions
{
    /// <summary>
    /// <see cref="MemoryHealthCheckSettings"/>
    /// </summary>
#pragma warning disable CS8618
    public MemoryHealthCheckSettings Settings { get; private set; }
#pragma warning restore CS8618

    /// <summary>
    /// set if memory heath check is enabled
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    public MemoryHealthCheckOptions WithEnabled(bool enabled)
    {
        Settings.Enabled = enabled;
        return this;
    }

    /// <summary>
    /// set the threshold
    /// </summary>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public MemoryHealthCheckOptions WithThreshold(long threshold)
    {
        Settings.Threshold = threshold;
        return this;
    }

    /// <summary>
    /// set the health check name
    /// </summary>
    /// <param name="healthCheckName"></param>
    /// <returns></returns>
    public MemoryHealthCheckOptions WithHealthCheckName(string healthCheckName)
    {
        Settings.MemoryHealthCheckName = healthCheckName;
        return this;
    }

    /// <summary>
    /// set the Health check full name
    /// </summary>
    /// <param name="healthCheckFullName"></param>
    /// <returns></returns>
    public MemoryHealthCheckOptions WithMemoryHealthCheckFullName(string healthCheckFullName)
    {
        Settings.MemoryHealthCheckFullName = healthCheckFullName;
        return this;
    }
}