using Oluso.Logger.Abstractions;

namespace Oluso.Logger.Serilog.Settings;

/// <summary>
/// Seq server settings 
/// </summary>
public class SerilogSeqSettings
{
    /// <summary>
    /// Default server url. default seq://localhost
    /// </summary>
    public const string DefaultServerUrl = "seq://localhost";
    
    /// <summary>
    /// Default Seq serve api key. default seq-logging
    /// </summary>
    public const string DefaultApiKey = "seq-logging";
    
    /// <summary>
    /// Default minimum logging level, default Error
    /// </summary>
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Error;

    private bool _enabled = false;
    private string _serverUrl = DefaultServerUrl;
    private string _apiKey = DefaultApiKey;
    private string _minimumLevel = DefaultMinimumLevel.ToString();
    private HttpMessageHandler? _messageHandler;

    /// <summary>
    /// get/set if logger is enabled
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
    /// get/set the Seq server connection url
    /// </summary>
    public string? ServerUrl
    {
        get => _serverUrl;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _serverUrl = value;
        }
    }

    /// <summary>
    /// get/set the api key in the Seq server
    /// </summary>
    public string? ApiKey
    {
        get => _apiKey;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _apiKey = value;
        }
    }

    /// <summary>
    /// get/set the Logger minimum level
    /// </summary>
    public string? MinimumLevel
    {
        get => _minimumLevel;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _minimumLevel = value;
        }
    }

    /// <summary>
    /// get/set the HttpMessageHandler
    /// </summary>
    public HttpMessageHandler? MessageHandler
    {
        get => _messageHandler;
        set
        {
            if (value != null)
                _messageHandler = value;
        }
    }
}