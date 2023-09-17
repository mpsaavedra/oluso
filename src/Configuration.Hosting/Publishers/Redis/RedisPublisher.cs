using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Oluso.Configuration.Abstractions;

namespace Oluso.Configuration.Hosting.Publishers.Redis;

/// <inheritdoc/>
public class RedisPublisher : IPublisher
{
    private readonly ILogger<RedisPublisher> _logger;
    private readonly ConfigurationOptions _options;
    private static IConnectionMultiplexer _connection = null!;

    /// <summary>
    /// returns a new <see cref="RedisPublisher"/> instance
    /// </summary>
    public RedisPublisher(ILogger<RedisPublisher> logger, ConfigurationOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        using (var writer = new StringWriter())
        {
            _connection = ConnectionMultiplexer.Connect(_options, writer);
            _logger.LogDebug(writer.ToString());
            _connection.ErrorMessage += (sender, args) =>
            {
                _logger.LogError(args.Message);
            };
            _connection.ConnectionFailed += (sender, args) =>
            {
                _logger.LogError("Connection to Redis message broker failed");
            };
            _connection.ConnectionRestored += (sender, args) =>
            {
                _logger.LogDebug("Connection to Redis message broker restored");
            };
            _logger.LogDebug("Redis configuration publisher initialized");
        }
    }

    /// <inheritdoc/>
    public async Task Publish(string topic, string message)
    {
        _logger.LogDebug("Publishing message to channel {channel}", topic);
        var publisher = _connection.GetSubscriber();
        var clientCount = await publisher.PublishAsync(topic, message);
        _logger.LogDebug("Changes published in {Topic} on channel {Channel}", topic, topic);
    }

}