namespace Oluso.HealthCheck.Settings;

/// <summary>
/// Health check settings
/// </summary>
public class HealthCheckSettings
{
    /// <summary>
    /// if true the health check is enabled
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// Memory health check settings
    /// </summary>
    public MemoryHealthCheckSettings? MemoryHealthCheckSettings { get; set; } = new();
}