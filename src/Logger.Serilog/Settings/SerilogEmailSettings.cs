using Oluso.Logger.Abstractions;

namespace Oluso.Logger.Serilog.Settings;

/// <summary>
/// Logging email notification settings
/// </summary>
public class SerilogEmailSettings
{
    /// <summary>
    /// Default email subject, default Log Error
    /// </summary>
    public const string DefaultSubject = "Log Error";

    /// <summary>
    /// 
    /// Default Ssl usage in connections, default true, 
    /// </summary>
    public const bool DefaultEnableSsl = true;

    /// <summary>
    /// Html body on emails. Default false
    /// </summary>
    public const bool DefaultIsBodyHtml = false;

    /// <summary>
    /// Default email server
    /// </summary>
    public const string DefaultServer = "localhost";

    /// <summary>
    /// Default port, default 485
    /// </summary>
    public const int DefaultPort = 485;

    /// <summary>
    /// Default out put template
    /// </summary>
    public const string DefaultOutputTemplate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | {Title}  [{Level:u}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// Default logger level, default Error
    /// </summary>
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Error;

    private bool _enabled;
    private bool _enabledSsl = DefaultEnableSsl;
    private int _port = DefaultPort;
    private bool _isBodyHtml = DefaultIsBodyHtml;
    private string _subject = DefaultSubject;
    private string _minimumLevel = DefaultMinimumLevel.ToString();
    private string _outputTemplate = DefaultOutputTemplate;

    /// <summary>
    /// get/set Username to connect to the server
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// get/set Password to connect to the server
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// get/set server to send the email
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// Email address that send the log
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Email addresses to send logs to
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// get/set if Email logging is enable
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
    /// get/set if Ssl is enabled
    /// </summary>
    public bool? EnableSsl
    {
        get => this._enabledSsl;
        set
        {
            if (value.HasValue)
            {
                this._enabledSsl = value.Value;
            }
        }
    }

    /// <summary>
    /// get/set the email send port
    /// </summary>
    public int? Port
    {
        get => this._port;
        set
        {
            if (value.HasValue)
            {
                this._port = value.Value;
            }
        }
    }

    /// <summary>
    /// get/set if Html body is enable
    /// </summary>
    public bool? IsBodyHtml
    {
        get => this._isBodyHtml;
        set
        {
            if (value.HasValue)
            {
                this._isBodyHtml = value.Value;
            }
        }
    }

    /// <summary>
    /// get/set the email subject
    /// </summary>
    public string? Subject
    {
        get => this._subject;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this._subject = value;
            }
        }
    }

    /// <summary>
    /// get/set the minimum logging level
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
    /// get/set the output template
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