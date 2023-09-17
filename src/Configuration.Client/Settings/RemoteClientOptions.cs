using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Client.Subscribers.RabbitMq;
using RedisOptions = StackExchange.Redis.ConfigurationOptions;

namespace Oluso.Configuration.Client.Settings;

/// <summary>
/// remote client options
/// </summary>
public class RemoteClientOptions
{
    /// <summary>
    /// <see cref="RemoteClientSettings"/> object
    /// </summary>
    public RemoteClientSettings Settings { get; set; } = new();

    /// <summary>
    /// set the server Uri from where to gather settings
    /// </summary>
    /// <param name="serverUri"></param>
    /// <returns></returns>
    public RemoteClientOptions WithServerUri(string serverUri)
    {
        return this;
    }

    /// <summary>
    /// add a list of <see cref="ConfigurationFileSettings"/>
    /// </summary>
    /// <param name="fileSettings"></param>
    /// <returns></returns>
    public RemoteClientOptions WithConfigurationFileSetting(params ConfigurationFileSettings[] fileSettings)
    {
        foreach (var arg in fileSettings)
        {
            Settings.ConfigurationFileSettingsList.Add(arg);
        }
        return this;
    }

    /// <summary>
    /// add a new <see cref="ConfigurationOptions"/>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public RemoteClientOptions WithConfiguration(params ConfigurationOptions[] options)
    {
        foreach (var option in options)
        {
            Settings.Configurations.Add(option);
        }
        return this;
    }

    /// <summary>
    /// set the Subscriber
    /// </summary>
    /// <param name="subscriber"></param>
    /// <returns></returns>
    public RemoteClientOptions WithSubscriber(ISubscriber subscriber)
    {
        return this;
    }

    /// <summary>
    /// set the <see cref="ILoggerFactory"/> to create logger
    /// </summary>
    /// <param name="factory"></param>
    /// <returns></returns>
    public RemoteClientOptions WithLoggerFactory(ILoggerFactory factory)
    {
        return this;
    }

    /// <summary>
    /// set the redis options from <see cref="RedisOptions"/>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public RemoteClientOptions WithRedisOptions(RedisOptions options)
    {
        return this;
    }

    /// <summary>
    /// set the redis options from a comma separed string
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public RemoteClientOptions WithRedisOptions(string options)
    {
        return this;
    }

    /// <summary>
    /// set the <see cref="RabbitMqSubscriberOptions"/>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public RemoteClientOptions WithRabbitMqOptions(RabbitMqSubscriberOptions options)
    {
        return this;
    }
}