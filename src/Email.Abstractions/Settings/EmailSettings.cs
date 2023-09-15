namespace Oluso.Email.Settings;

/// <summary>
/// Email settings interface
/// </summary>
public interface IEmailSettings
{
    /// <summary>
    /// if true credentials will be used to authenticate into server
    /// </summary>
    bool? UseCredentials { get; set; }
    
    /// <summary>
    /// if true service is available to be used
    /// </summary>
    bool? Enable { get; set; }
    
    /// <summary>
    /// get set the user name to connect to server
    /// </summary>
    string? UserName { get; set; }
    
    /// <summary>
    /// get/set the user password to connect to server
    /// </summary>
    string? Password { get; set; }
    
    /// <summary>
    /// get/set the server host
    /// </summary>
    string? Host { get; set; }
    
    /// <summary>
    /// get/set the server port
    /// </summary>
    int? Port { get; set; }
    
    /// <summary>
    /// get/set if use Ssl to connect to server
    /// </summary>
    bool? UseSsl { get; set; }
    
    /// <summary>
    /// get/set the templates directory
    /// </summary>
    string? TemplatesDirectory { get; set; }
    
    /// <summary>
    /// attachments directory's path 
    /// </summary>
    string? AttachmentPathDirectory { get; set; }
    
    /// <summary>
    /// email provider name
    /// </summary>
    string? Provider { get; set; }
}

/// <summary>
/// Email settings
/// </summary>
public class EmailSettings : IEmailSettings
{
    /// <summary>
    /// default server host
    /// </summary>
    public const string DefaultHost = "localhost";
    
    /// <summary>
    /// default server port
    /// </summary>
    public const int DefaultPort = 445;
    
    /// <summary>
    /// default if use Ssl or not
    /// </summary>
    public const bool DefaultUseSsl = true;
    
    /// <summary>
    /// default templates directory
    /// </summary>
    public const string DefaultTemplatesDirectory = @"templates";
    
    /// <summary>
    /// default if service is enabled
    /// </summary>
    public const bool DefaultEnabled = false;
    
    /// <summary>
    /// default is use credentials or not
    /// </summary>
    public const bool DefaultUseCredentials = false;
    
    /// <summary>
    /// default template's directory
    /// </summary>
    public const string DefaultAttachmentPathDirectory = @"attachments";
    
    /// <summary>
    /// default email provider
    /// </summary>
    public const string DefaultEmailProvider = "Custom"; 

    private bool _useCredentials = DefaultUseCredentials;
    private bool _enabled = DefaultEnabled;
    private string _userName = "";
    private string _pasword = " ";
    private string _host = DefaultHost;
    private int _port = DefaultPort;
    private bool _useSsl = DefaultUseSsl;
    private string _templatesDirectory = DefaultTemplatesDirectory;
    private string _attachmentDirectory = DefaultAttachmentPathDirectory;
    private string _provider = EmailProvider.Custom.Name;

    /// <inheritdoc cref="IEmailSettings.Provider"/>
    public string? Provider
    {
        get => _provider;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _provider = value;
        }
    }
    
    /// <inheritdoc cref="IEmailSettings.AttachmentPathDirectory"/>
    public string? AttachmentPathDirectory
    {
        get => _attachmentDirectory;
        set
        {
            if(!string.IsNullOrEmpty(value))
                _attachmentDirectory = value;
        }
    }

    /// <inheritdoc cref="IEmailSettings.UseCredentials"/>
    public bool? UseCredentials
    {
        get => _useCredentials;
        set
        {
            if (value.HasValue)
                _useCredentials = value.Value;
        }
    }
    
    /// <see cref="IEmailSettings.Enable"/>
    public bool? Enable
    {
        get => _enabled;
        set
        {
            if (value.HasValue)
                _enabled = value.Value;
        }
    }
    
    /// <inheritdoc cref="IEmailSettings.UserName"/>
    public string? UserName
    {
        get => _userName;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _userName = value;
        }
    }
    
    /// 
    public string? Password
    {
        get => _pasword;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _pasword = value;
        }
    }

    /// <inheritdoc cref="IEmailSettings.Host"/>
    public string? Host
    {
        get => _host;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _host = value;
        }
    }

    /// <see cref="IEmailSettings.Port"/>
    public int? Port
    {
        get => _port;
        set
        {
            if (value.HasValue)
            {
                _port = value.Value;
            }
        }
    }

    /// <inheritdoc cref="IEmailSettings.UseSsl"/>
    public bool? UseSsl
    {
        get => _useSsl;
        set
        {
            if (value.HasValue)
                _useSsl = value.Value;
        }
    }

    /// <inheritdoc cref="IEmailSettings.TemplatesDirectory"/>
    public string? TemplatesDirectory
    {
        get => _templatesDirectory;
        set
        {
            if (!string.IsNullOrWhiteSpace(_templatesDirectory) && value != null)
                _templatesDirectory = value;
            throw new ApplicationException("Template directory could not be null or whitespace");
        }
    }
}