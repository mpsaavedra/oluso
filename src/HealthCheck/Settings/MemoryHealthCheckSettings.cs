namespace Oluso.HealthCheck.Settings;

/// <summary>
/// Memory health check settings
/// </summary>
public class MemoryHealthCheckSettings
{
    /// <summary>
    /// health check threshold
    /// </summary>
    public const long DefaultThreshold  = 1073741824;

    /// <summary>
    /// name of the memory health check
    /// </summary>
    public const string DefaultMemoryHealthCheckName = "memory";

    /// <summary>
    /// full name
    /// </summary>
    public const string DefaultMemoryHealthCheckFullName = "hosted_service_startup";

    private bool _enabled;
    private long _threshold = DefaultThreshold;
    private string _memoryHealthCheckName = DefaultMemoryHealthCheckName;
    private string _memoryHealthCheckFullName = DefaultMemoryHealthCheckFullName;

    /// <summary>
    /// if true memory check is enabled
    /// </summary>
    public bool? Enabled
    {
        get => _enabled;
        set
        {
            if (value.HasValue)
                _enabled = value.Value;
        }
    }

    /// <summary>
    /// health check threshold
    /// </summary>
    public long? Threshold
    {
        get => _threshold;
        set
        {
            if (value.HasValue)
                _threshold = value.Value;
        }
    }

    /// <summary>
    /// name of the memory health check
    /// </summary>
    public string? MemoryHealthCheckName
    {
        get => _memoryHealthCheckName;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _memoryHealthCheckName = value;
        }
    }

    /// <summary>
    /// full name
    /// </summary>
    public string? MemoryHealthCheckFullName
    {
        get => _memoryHealthCheckFullName;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _memoryHealthCheckFullName = value;
        }
    }
}