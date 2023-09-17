using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Configuration.Abstractions.Extensions;
using Oluso.Configuration.Client.Settings;

namespace Oluso.Configuration.Client.Extensions;

/// <summary>
/// Remote configuration configuration
/// </summary>
public static class RemoteConfigurationExtensions
{
    /// <summary>
    /// add remote configuration client
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <param name="returnServices">if true return type will be IServiceCollection otherwise IConfigurationRoot</param>
    /// <returns></returns>
    public static dynamic AddRemoteConfiguration(
        this IServiceCollection services,
        Action<RemoteClientOptions> options,
        bool returnServices = true) =>
        services.AddRemoteConfiguration(options!.ToConfigurationConfigureOrDefault().Settings, returnServices);


    /// <summary>
    /// add remote configuration client
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <param name="returnServices">if true return type will be IServiceCollection otherwise IConfigurationRoot</param>
    /// <returns></returns>
    public static dynamic AddRemoteConfiguration(
        this IServiceCollection services,
        RemoteClientSettings settings,
        bool returnServices = true)
    {
        var localBuilder = new ConfigurationBuilder();
        foreach (var cfgFile in settings.ConfigurationFileSettingsList)
        {
            localBuilder.AddJsonFile(cfgFile.FileName, cfgFile.Optional, cfgFile.ReloadOnChange);
        }

        var localConfiguration = localBuilder.Build();

        var configuration = new ConfigurationBuilder()
            .AddConfiguration(localConfiguration);

        configuration.AddRemoteConfiguration(opts =>
        {
            opts.ServiceUri = settings.ServiceUri;

            foreach (var cfg in settings.Configurations)
            {
                opts.AddConfiguration(c =>
                {
                    c.ConfigurationName = cfg.ConfigurationName;
                    c.ReloadOnChange = cfg.ReloadOnChange;
                    c.Optional = cfg.Optional;
                    if (cfg.Parser != null)
                        c.Parser = cfg.Parser;
                });
            }

            if (settings.RedisOptions != null)
            {
                opts.AddRedisSubscriber(settings.RedisOptions);
            }
            else if (!string.IsNullOrEmpty(settings.RedisConfiguration) ||
                     !string.IsNullOrWhiteSpace(settings.RedisConfiguration))
            {
                opts.AddRedisSubscriber(settings.RedisConfiguration!);
            }

            if (settings.RabbitMqOptions != null)
            {
                opts.AddRabbitMqSubscriber(settings.RabbitMqOptions);
            }

            if (settings.LoggerFactory != null)
                opts.AddLoggerFactory(settings.LoggerFactory);
        });

        configuration.Build();
        return returnServices ? services : configuration;
    }

    /// <summary>
    /// Adds a remote configuration source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configure">Configures the source.</param>
    /// <returns></returns>
    public static IConfigurationBuilder AddRemoteConfiguration(
        this IConfigurationBuilder builder,
        Action<RemoteConfigurationOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new RemoteConfigurationOptions();
        configure(options);

        var remoteBuilder = new RemoteConfigurationBuilder(builder, options);
        return remoteBuilder;
    }
}