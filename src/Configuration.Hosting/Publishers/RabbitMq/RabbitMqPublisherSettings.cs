namespace Oluso.Configuration.Hosting.Publishers.RabbitMq;

/// <summary>
/// RabbitMq publisher settings
/// </summary>
public class RabbitMqPublisherSettings
{
    /// <summary>
    ///  server to connect to
    /// </summary>   
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// virtual host to access during connection,
    /// Default is "/"
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Username to authenticate user into server. Default
    /// to "guest"
    /// </summary>
    public string Username { get; set; } = "guest";

    /// <summary>
    /// password to authenticate user into server. Default
    /// to "guest"
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// name of the fan out exchange. Default to "configuration-service"
    /// </summary>
    public string ExchangeName { get; set; } = "configuration-service";
}