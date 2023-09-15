using System.Text;
using Oluso.Configuration.Abstractions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Oluso.Configuration.Hosting.Publishers.RabbitMq;

/// <summary>
/// <inheritdoc/>. RabbitMq publisher implementation.
/// </summary>
public class RabbitMqPublisher : IPublisher
{
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly RabbitMqPublisherSettings _settings;
    private string _exchangeName;

#pragma warning disable CS8618
    private static IModel _channel;

    /// <summary>
    /// returns a new <see cref="RabbitMqPublisher"/> instance
    /// </summary>
    public RabbitMqPublisher(ILogger<RabbitMqPublisher> logger,
        RabbitMqPublisherSettings settings)
    {
#pragma warning restore CS8618
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger;
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        _logger.LogDebug("Initializing RabbitMq publisher");
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            VirtualHost = _settings.VirtualHost,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _exchangeName = _settings.ExchangeName;

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        connection.CallbackException += (sender, args) => { _logger.LogError(args.Exception, "RabbitMQ callback exception."); };
        connection.ConnectionBlocked += (sender, args) => { _logger.LogError("RabbitMQ connection is blocked. Reason: {Reason}", args.Reason); };
        connection.ConnectionShutdown += (sender, args) => { _logger.LogError("RabbitMQ connection shut down. Reason: {ReplyText}", args.ReplyText); };
        connection.ConnectionUnblocked += (sender, args) => { _logger.LogInformation("RabbitMQ connection was unblocked."); };

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
        _logger.LogDebug("RabbitMq publisher initialized");
    }

    /// <inheritdoc/>
    public Task Publish(string topic, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(_exchangeName, topic, null, body);

        _logger.LogDebug("Changes published in {Topic} on channel {Channel}", _exchangeName, topic);
        return Task.CompletedTask;
    }
}