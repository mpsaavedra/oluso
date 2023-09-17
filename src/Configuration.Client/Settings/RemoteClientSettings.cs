using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Client.Subscribers.RabbitMq;
using RedisOptions = StackExchange.Redis.ConfigurationOptions;

namespace Oluso.Configuration.Client.Settings;

#pragma warning disable CS8618

/// <summary>
/// Remote client settings
/// </summary>
public class RemoteClientSettings
{
    /// <summary>
    /// remote configuration service Uri
    /// </summary>
    public string ServiceUri { get; set; }

    /// <summary>
    /// optional configuration files to load
    /// </summary>
    public List<ConfigurationFileSettings> ConfigurationFileSettingsList { get; set; } = new();
    
    /// <summary>
    /// Subscriber to use to read configuration
    /// </summary>
    public ISubscriber Subscriber { get; set; }

    /// <summary>
    /// List of configuration to load from remote provider
    /// </summary>
    public List<ConfigurationOptions> Configurations { get; set; } = new();

    /// <summary>
    /// Logger configuration
    /// </summary>
    public ILoggerFactory? LoggerFactory { get; set; } = null;

    /// <summary>
    /// <see cref="RedisOptions"/> configuration to configure Redis subscriber
    /// </summary>
    public Action<RedisOptions>? RedisOptions { get; set; } = null;

    /// <summary>
    /// Redis configuration as string comma separated to configure Redis subscriber
    /// </summary>
    public string? RedisConfiguration { get; set; } = null;

    /// <summary>
    /// <see cref="RabbitMqSubscriberOptions"/> to configure RabbitMq subscriber
    /// </summary>
    public Action<RabbitMqSubscriberOptions>? RabbitMqOptions { get; set; } = null;
}

/// <summary>
/// configuration files settings
/// </summary>
public class ConfigurationFileSettings
{
    /// <summary>
    /// name of configuration file
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// settings file is optional or not
    /// </summary>
    public bool Optional { get; set; }
    
    /// <summary>
    /// Reload configuration on file change
    /// </summary>
    public bool ReloadOnChange { get; set; }

    /// <summary>
    /// returns a new <see cref="ConfigurationFileSettings"/>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="optional"></param>
    /// <param name="reloadOnChange"></param>
    /// <returns></returns>
    public static ConfigurationFileSettings Create(string fileName, bool optional, bool reloadOnChange)
    {
        return new()
        {
            FileName = fileName,
            Optional = optional,
            ReloadOnChange = reloadOnChange
        };
    }
}

#pragma warning restore CS8618
