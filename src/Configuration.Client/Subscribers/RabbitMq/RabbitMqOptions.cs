namespace Oluso.Configuration.Client.Subscribers.RabbitMq;

/// <summary>
/// Rabbit Mq connection options
/// </summary>
public class RabbitMqSubscriberOptions
{
    /// <summary>The host to connect to.
    /// Defaults to "localhost".</summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Virtual host to access during this connection.
    /// Defaults to "/".
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Username to use when authenticating to the server.
    /// Defaults to "guest".
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Password to use when authenticating to the server.
    /// Defaults to "guest".
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Name of the fan out exchange.
    /// Defaults to "configuration-service"
    /// </summary>
    public string ExchangeName { get; set; } = "configuration-service";
}