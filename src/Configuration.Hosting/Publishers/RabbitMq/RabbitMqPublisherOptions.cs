using Oluso.Configuration.Abstractions.Extensions;

namespace Oluso.Configuration.Hosting.Publishers.RabbitMq;

/// <summary>
/// RabbitMq 
/// </summary>
public class RabbitMqPublisherOptions
{
    /// <summary>
    /// <see cref="RabbitMqPublisherSettings"/>
    /// </summary>
    public RabbitMqPublisherSettings Settings { get; } = new();

    /// <summary>
    /// returns a new <see cref="RabbitMqPublisherOptions"/> instance
    /// </summary>
    public RabbitMqPublisherOptions()
    {
    }

    /// <summary>
    /// set the RabbitMq server hostname
    /// </summary>
    /// <param name="hostname"></param>
    /// <returns></returns>
    public RabbitMqPublisherOptions WithHostName(string hostname)
    {
        hostname.ToNullEmptyOrWhitespaceThrow("Hostname could not be null empty or whitespace");
        Settings.HostName = hostname;
        return this;
    }

    /// <summary>
    /// virtual host to publish message into
    /// </summary>
    /// <param name="virtualHost"></param>
    /// <returns></returns>
    public RabbitMqPublisherOptions WithVirtualHost(string virtualHost)
    {
        Settings.VirtualHost = virtualHost;
        return this;
    }

    /// <summary>
    /// set the username to access to the RabbitMq server
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public RabbitMqPublisherOptions WithUserName(string username)
    {
        Settings.Username = username;
        return this;
    }

    /// <summary>
    /// set the password to access to the RabbitMq Server
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public RabbitMqPublisherOptions WithPassword(string password)
    {
        Settings.Password = password;
        return this;
    }

    /// <summary>
    /// set the exchange name
    /// </summary>
    /// <param name="exchangeName"></param>
    /// <returns></returns>
    public RabbitMqPublisherOptions WithExchangeName(string exchangeName)
    {
        Settings.ExchangeName = exchangeName;
        return this;
    }
}