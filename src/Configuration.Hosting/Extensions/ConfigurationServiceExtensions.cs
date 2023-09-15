using Microsoft.Extensions.DependencyInjection;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Hosting.Providers.Filesystem;
using Oluso.Configuration.Hosting.Publishers.RabbitMq;
using Oluso.Configuration.Hosting.Publishers.Redis;
using StackExchange.Redis;

namespace Oluso.Configuration.Hosting.Extensions;

/// <summary>
/// Service collection related extensions
/// </summary>
public static class ConfigurationServiceExtensions
{
    /// <summary>
    /// add hosted configuration service
    /// </summary>
    public static IConfigurationServiceBuilder AddConfigurationServiceBuilder(this IServiceCollection services)
    {
        services.AddHostedService<HostedConfigurationService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        return new ConfigurationServiceBuilder(services);
    }

    /// <summary>
    /// add filesystem provider.
    /// </summary>
    public static IConfigurationServiceBuilder AddFilesystemProvider(this IConfigurationServiceBuilder builder,
        Action<FilesystemProviderOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var opts = new FilesystemProviderOptions();
        configure(opts);
        builder.Services.AddSingleton(opts.Settings);
        builder.Services.AddSingleton<IProvider, FilesystemProvider>();
        return builder;
    }


    /// <summary>
    /// register a RabbitMq publisher tot he services
    /// </summary>
    public static IConfigurationServiceBuilder AddRabbitMqPublisher(this IConfigurationServiceBuilder builder,
        Action<RabbitMqPublisherOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new RabbitMqPublisherOptions();
        configure(options);

        builder.Services.AddSingleton(options.Settings);
        builder.Services.AddSingleton<IPublisher, RabbitMqPublisher>();

        return builder;
    }

    /// <summary>
    ///   adds a <see cref="RedisPublisher"/> to the services
    /// </summary>
    public static IConfigurationServiceBuilder AddRedisPublisher(this IConfigurationServiceBuilder builder,
        Action<ConfigurationOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new ConfigurationOptions();
        configure(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IPublisher, RedisPublisher>();

        return builder;
    }

    /// <summary>
    /// register a Redis publisher to the services
    /// </summary>
    public static IConfigurationServiceBuilder AddRedisPublisher(this IConfigurationServiceBuilder builder,
        string configurationEntry)
    {
        if (string.IsNullOrEmpty(configurationEntry))
        {
            throw new ArgumentNullException(nameof(configurationEntry));
        }

        var options = ConfigurationOptions.Parse(configurationEntry);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IPublisher, RedisPublisher>();

        return builder;
    }

    /// <summary>
    /// register a new custom provider implementation to the services as a singleton service
    /// </summary>
    public static IConfigurationServiceBuilder AddProvider(this IConfigurationServiceBuilder builder,
        IProvider provider)
    {
        builder.Services.AddSingleton(provider);
        return builder;
    }

    /// <summary>
    ///  add a new custom publisher implementation to the services as a singleton service
    /// </summary>
    public static IConfigurationServiceBuilder AddPublisher(this IConfigurationServiceBuilder builder,
        IPublisher publisher)
    {
        builder.Services.AddSingleton(publisher);
        return builder;
    }
}