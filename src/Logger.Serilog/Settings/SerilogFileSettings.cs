using Oluso.Logger.Abstractions;

namespace Oluso.Logger.Serilog.Settings;

/// <summary>
/// Logger file settings
/// </summary>
public class SerilogFileSettings
{
    /// <summary>
    /// default saving
    /// </summary>
    public const string DefaultFilePath = @"Logs\\.log";

    /// <summary>
    /// Default logging minimum level
    /// </summary>
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Error;

    /// <summary>
    /// Default file interval
    /// </summary>
    public const LoggerFileInterval DefaultLoggerInterval = LoggerFileInterval.Day;

    private bool _enabled;

    private string _filePath = DefaultFilePath;

    private string _minimumLevel = DefaultMinimumLevel.ToString();

    private string _interval = DefaultLoggerInterval.ToString();

    /// <summary>
    /// if true logging is enable to use
    /// </summary>
    public bool? Enabled
    {
        get => this._enabled;
        set
        {
            if (value.HasValue)
            {
                this._enabled = value.Value;
            }
        }
    }

    /// <summary>
    /// here to save logs
    /// </summary>
    public string? FilePath
    {
        get => this._filePath;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this._filePath = value;
            }
        }
    }

    /// <summary>
    /// Logging minimum level
    /// </summary>
    public string? MinimumLevel
    {
        get => this._minimumLevel;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this._minimumLevel = value;
            }
        }
    }

    /// <summary>
    /// Logging interval
    /// </summary>
    public string? Interval
    {
        get => this._interval;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this._interval = value;
            }
        }
    }
}