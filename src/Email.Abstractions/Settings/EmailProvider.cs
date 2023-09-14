namespace Oluso.Email.Settings;

/// <summary>
/// Common email provider information
/// </summary>
public class EmailProvider
{
    /// <summary>
    /// Default Gmail server data
    /// </summary>
    public static readonly EmailProvider Gmail = new("Gmail", "smtp.gmail.com", 587);
    
    /// <summary>
    /// default Hotmail server data
    /// </summary>
    public static readonly EmailProvider Hotmail = new("Hotmail", "smtp.live.com", 587);
    
    /// <summary>
    /// default Office365 server data
    /// </summary>
    public static readonly EmailProvider Office365 = new("Office365", "smtp.office365.com", 587);
    
    /// <summary>
    /// when using AWS you must set the AWS_REGION to connect to the server
    /// </summary>
    public static readonly EmailProvider Aws = new("Aws", "email-smtp.{AWS_REGION}.amazonaws.com", 587);
    
    /// <summary>
    /// you must provide information to use this provider
    /// </summary>
    public static readonly EmailProvider Custom = new("Custom", "", 587);
    
    /// <summary>
    /// provider canonical name
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// provider host
    /// </summary>
    public string Host { get; }
    
    /// <summary>
    /// Provider port
    /// </summary>
    public int Port { get; }

    private EmailProvider(string name, string host, int port)
    {
        Name = name;
        Host = host;
        Port = port;
    }
}