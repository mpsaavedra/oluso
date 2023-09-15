using System.Text;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Oluso.Configuration.Client.Subscribers.RabbitMq;

/// <summary>
/// RabbitMq subscriber, that connections to a Redis server to
/// listen for configuration changes
/// </summary>
public class RabbitMqSubscriber : ISubscriber
{
    private readonly ILogger<RabbitMqSubscriber> _logger;
    private readonly RabbitMqSubscriberOptions _options;
    private IModel _channel;
    private string _exchangeName;

#pragma warning disable CS8618
    /// <summary>
    /// returns a new <see cref="RabbitMqSubscriber"/>
    /// </summary>
    public RabbitMqSubscriber(RabbitMqSubscriberOptions options)
    {
#pragma warning restore CS8618
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _logger = Logger.CreateLogger<RabbitMqSubscriber>();
        _options = options;
    }

    /// <inheritdoc/>
    public string Name => "RabbitMq";

    /// <inheritdoc/>
    public void Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            VirtualHost = _options.VirtualHost,
            UserName = _options.UserName,
            Password = _options.Password
        };

        _exchangeName = _options.ExchangeName;

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        connection.CallbackException += (sender, args) => { _logger.LogError(args.Exception, "RabbitMQ callback exception."); };
        connection.ConnectionBlocked += (sender, args) => { _logger.LogError("RabbitMQ connection is blocked. Reason: {Reason}", args.Reason); };
        connection.ConnectionShutdown += (sender, args) => { _logger.LogError("RabbitMQ connection shut down. Reason: {ReplyText}", args.ReplyText); };
        connection.ConnectionUnblocked += (sender, args) => { _logger.LogInformation("RabbitMQ connection was unblocked."); };

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
        _logger.LogInformation("RabbitMq subscriber connection initialized");
    }

    /// <inheritdoc/>
    public void Subscribe(string channel, Action<string> handler)
    {
        _logger.LogInformation("Binding to RabbitMQ queue with channel '{channel}'.", channel);

        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queueName, _exchangeName, channel);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, args) =>
        {
            _logger.LogInformation("Received message with channel '{channel}'.", args.RoutingKey);

            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            handler(message);
        };

        var consumerTag = _channel.BasicConsume(queueName, true, consumer);

        _logger.LogInformation("Consuming RabbitMQ queue {queueName} for consumer '{consumerTag}'.", queueName, consumerTag);
    }
}