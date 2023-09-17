using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using RedisOptions = StackExchange.Redis.ConfigurationOptions;

namespace Oluso.Configuration.Client.Subscribers.Redis;

/// <summary>
/// redis subscriber, that connections to a Redis server to
/// listen for configuration changes
/// </summary>
public class RedisSubscriber : Oluso.Configuration.Abstractions.ISubscriber
{
    private readonly ILogger<RedisSubscriber> _logger;

    private readonly RedisOptions _options;

#pragma warning disable CS8618
    private static IConnectionMultiplexer _connection;
#pragma warning restore CS8618

    /// <inheritdoc/>
    public string Name => "Redis";

    /// <summary>
    /// return a new <see cref="RedisSubscriber"/> instance
    /// </summary>
    /// <param name="configuration">configuration entry with connection paramters</param>
    public RedisSubscriber(string configuration)
    {
        _logger = Logger.CreateLogger<RedisSubscriber>();

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        _options = RedisOptions.Parse(configuration);
    }

    /// <summary>
    /// returns a new <see cref="RedisSubscriber"/> instance
    /// </summary>
    /// <param name="options"><see cref="RedisOptions"/> object with connection parameters</param>
    public RedisSubscriber(RedisOptions options)
    {
        _logger = Logger.CreateLogger<RedisSubscriber>();

        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        using (var writer = new StringWriter())
        {
            _connection = ConnectionMultiplexer.Connect(_options, writer);
            _logger.LogDebug(writer.ToString());
        }

        _connection.ErrorMessage += (sender, args) => { _logger.LogError(args.Message); };
        _connection.ConnectionFailed += (sender, args) => { _logger.LogError(args.Exception, "Redis connection failed"); };
        _connection.ConnectionRestored += (sender, args) => { _logger.LogInformation("Redis connection restored"); };
    }

    /// <inheritdoc/>
    public void Subscribe(string channel, Action<string> handler)
    {
        _logger.LogInformation("Subscribing to channel {channel}", channel);

        var subscriber = _connection.GetSubscriber();

        subscriber.Subscribe(channel, (redisChannel, value) =>
        {
            _logger.LogInformation("Received subscription to channel {channel}", channel);
            handler(channel);
        });

        var endpoint = subscriber.SubscribedEndpoint(channel);
        _logger.LogInformation("Subscribed to Redis endpoint {endpoint} on channel {channel}", endpoint, channel);
    }
}