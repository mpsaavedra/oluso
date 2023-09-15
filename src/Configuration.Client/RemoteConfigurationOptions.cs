using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Client.Subscribers.RabbitMq;
using Oluso.Configuration.Client.Subscribers.Redis;
using RedisOptions = StackExchange.Redis.ConfigurationOptions;

namespace Oluso.Configuration.Client;

/// <summary>
///  Configuration and other options that allow to use and configure the
///  REmove configuration service subscriber.
/// </summary>
public class RemoteConfigurationOptions
{
    internal IList<ConfigurationOptions> Configurations { get; } = new List<ConfigurationOptions>();

#pragma warning disable CS8618
    internal Func<ISubscriber> CreateSubscriber { get; set; }

    /// <summary>
    /// Configuration service endpoint.
    /// </summary>
    public string ServiceUri { get; set; }

    /// <summary>
    /// The <see cref="System.Net.Http.HttpMessageHandler"/> for the <see cref="HttpClient"/>.
    /// </summary>
    public HttpMessageHandler HttpMessageHandler { get; set; }

    /// <summary>
    /// The timeout for the <see cref="HttpClient"/> request to the configuration server.
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// The type used to configure the logging system and create instances of <see cref="ILogger"/>
    /// </summary>
    public ILoggerFactory LoggerFactory { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Adds an individual configuration file.
    /// </summary>
    /// <param name="configure">Configures the options for the configuration file.</param>
    public void AddConfiguration(Action<ConfigurationOptions> configure)
    {
        var configurationOptions = new ConfigurationOptions();
        configure(configurationOptions);

        Configurations.Add(configurationOptions);
    }

    /// <summary>
    /// Adds the type used to configure the logging system and create instances of <see cref="ILogger"/>
    /// </summary>
    /// <param name="loggerFactory"></param>
    public void AddLoggerFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    /// <summary>
    /// Adds a custom subscriber.
    /// </summary>
    /// <param name="subscriberFactory">The delegate used to create the custom implementation of <see cref="ISubscriber"/>.</param>
    public void AddSubscriber(Func<ISubscriber> subscriberFactory)
    {
        if (CreateSubscriber != null)
        {
            throw new InvalidOperationException("A subscriber has already been configured.");
        }

        CreateSubscriber = subscriberFactory ?? throw new ArgumentNullException(nameof(subscriberFactory));
    }

    /// <summary>
    /// Adds RabbitMQ as the configuration subscriber.
    /// </summary>
    /// <param name="configure">Configure options for the RabbitMQ subscriber.</param>
    public void AddRabbitMqSubscriber(Action<RabbitMqSubscriberOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        if (CreateSubscriber != null)
        {
            throw new InvalidOperationException("A subscriber has already been configured.");
        }

        var options = new RabbitMqSubscriberOptions();
        configure(options);

        CreateSubscriber = () => new RabbitMqSubscriber(options);
    }

    /// <summary>
    /// Adds Redis as the configuration subscriber.
    /// </summary>
    /// <param name="configure">Configure options for the Redis multiplexer.</param>
    public void AddRedisSubscriber(Action<RedisOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        if (CreateSubscriber != null)
        {
            throw new InvalidOperationException("A subscriber has already been configured.");
        }

        var options = new RedisOptions();
        configure(options);

        CreateSubscriber = () => new RedisSubscriber(options);
    }

    /// <summary>
    /// Adds Redis as the configuration subscriber.
    /// </summary>
    /// <param name="configuration">The string configuration for the Redis multiplexer.</param>
    public void AddRedisSubscriber(string configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (CreateSubscriber != null)
        {
            throw new InvalidOperationException("A subscriber has already been configured.");
        }

        var options = RedisOptions.Parse(configuration);

        CreateSubscriber = () => new RedisSubscriber(options);
    }
}