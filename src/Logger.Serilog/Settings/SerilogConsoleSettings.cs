using Oluso.Logger.Abstractions;

namespace Oluso.Logger.Serilog.Settings;

/// <summary>
/// Console logger settings
/// </summary>
public class SerilogConsoleSettings
{
    /// <summary>
    /// Default logging template,
    /// </summary>
    public const string DefaultOutputTemplate = "{NewLine}{Timestamp:HH:mm:ss:fff} | {Title} {Level:u3} {Message:lj}";

    /// <summary>
    /// Default minimum logging level, Info by default 
    /// </summary>
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Info;

    private bool _enabled;

    private string _minimumLevel = DefaultMinimumLevel.ToString();

    private string _outputTemplate = DefaultOutputTemplate;

    /// <summary>
    /// if true logger will be use
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
    /// minimum logging level
    /// </summary>
    public string MinimumLevel
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
    /// Output template is the format used to display information into the console
    /// </summary>
    public string? OutputTemplate
    {
        get => this._outputTemplate;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this._outputTemplate = value;
            }
        }
    }
}