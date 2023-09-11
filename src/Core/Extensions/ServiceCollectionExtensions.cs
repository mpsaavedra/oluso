#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// return the <see cref="IHostEnvironment"/>
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static IHostEnvironment ToEnvironment(this IServiceProvider provider) =>
        provider.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(provider))).GetService<IHostEnvironment>()!;

    /// <summary>
    /// returns the <see cref="IHostEnvironment"/>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IHostEnvironment ToEnvironment(this IServiceCollection services) =>
        services.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(services))).ToService<IHostEnvironment>()!;

    /// <summary>
    /// gets the requested service
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService? ToService<TService>(this IServiceCollection services) =>
        services.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(services))).BuildServiceProvider()
            .GetService<TService>();

    /// <summary>
    /// register all services which classes match with their interface, (ClassName match with IClassName)
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection ToServiceMatchingInterface(
        this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var errors = new List<string>();
        if (Is.NullOrEmpty(services))
        {
            errors.Add(Messages.NullOrEmpty(nameof(services)));
        }

        if (Is.NullOrAnyNull(assemblies))
        {
            errors.Add(Messages.NullOrAnyNull(nameof(assemblies)));
        }

        if (errors.Any())
        {
            Insist.Throw<Exception>(errors);
        }

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface((service, filter) =>
                filter.Where(impl => impl.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase)))
            .WithLifetime(lifetime));
        
        return services;
    }

    /// <summary>
    /// check if some service exists (is registered)
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static bool ToExists<TService>(this IServiceCollection services) =>
        services.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(services))).ToService<TService>() != null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="directoryPath"></param>
    /// <param name="files"></param>
    /// <returns></returns>
    public static IServiceCollection ToConfiguration(this IServiceCollection services, string directoryPath,
        params (string jsonFilename, bool options, bool reloadOnCHange)[] files)
    {
        var errors = new List<string>();
        if (Is.NullOrEmpty(errors))
        {
            errors.Add(Messages.NullOrEmpty(nameof(services)));
        }

        if (Is.NullOrEmpty(directoryPath))
        {
            errors.Add(Messages.NullOrEmpty(nameof(directoryPath)));
        }
        else
        {
            if (!Is.Directory(directoryPath))
            {
                errors.Add(Messages.NotFound(directoryPath));
            }
        }

        if (Is.NullOrAnyNull(files))
        {
            errors.Add(Messages.NullOrAnyNull(nameof(files)));
        }

        if (errors.Any())
        {
            Insist.Throw<Exception>(errors);
        }

        if (files != null)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(directoryPath)
                .ToJsonFiles(directoryPath, files)
                .AddEnvironmentVariables()
                .Build();
            services.ToConfiguration(configurationBuilder);
        }
        
        return services;
    }

    /// <summary>
    /// add configuration services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ToConfiguration(
        this IServiceCollection services,
        params (string key, string? value)[] configuration)
    {
        var errors = new List<string>();

        if (Is.NullOrEmpty(services))
        {
            errors.Add(Messages.NullOrEmpty(nameof(services)));
        }

        if (Is.NullOrAnyNull(configuration))
        {
            errors.Add(Messages.NullOrAnyNull(nameof(configuration)));
        }

        Insist.ThrowAny<Exception>(errors);
        
        var list = new Dictionary<string, string?>();
        foreach (var (key, value) in configuration)
        {
            list.Add(key, value);
        }

        var configurationBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(list)
            .Build();
        services.ToConfiguration(configurationBuilder);
        return services;
    }

    /// <summary>
    /// add configuration services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ToConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(services))).TryAddSingleton(configuration);
        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="defaultSettings"></param>
    /// <typeparam name="TSettings"></typeparam>
    /// <typeparam name="TConfigureSettings"></typeparam>
    /// <returns></returns>
    public static TSettings? ToSettings<TSettings, TConfigureSettings>(this IServiceCollection services,
        Action<TSettings>? defaultSettings = null)
        where TSettings : class, new()
        where TConfigureSettings : class, IConfigureOptions<TSettings>
    {
        if (defaultSettings is null)
        {
            services.ToSettings((TSettings)null!);
        }
        else
        {
            services.ToSettings(defaultSettings!.ToConfigureOrDefault());
        }

        var result = services
            .AddSingleton<IConfigureOptions<TSettings>>()
            .ToService<IOptions<TSettings>>()?
            .Value;

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="defaultSettings"></param>
    /// <typeparam name="TSettings"></typeparam>
    /// <returns></returns>
    public static TSettings ToSettings<TSettings>(this IServiceCollection services, TSettings? defaultSettings = default)
        where TSettings : class, new()
    {
        TSettings? settings;
        if (!services.ToExists<TSettings>())
        {
            services.AddOptions<TSettings>();
            var section = services.ToService<IConfiguration>()?.GetSection(nameof(TSettings));
            if (section.Exists())
            {
                services.Configure<TSettings>(section);
                var service = services.ToService<IOptions<TSettings>>();
                settings = service?.Value;

                if (defaultSettings is not null)
                {
                    settings = settings?.ToMapUpdate(defaultSettings);
                }
            }
            else
            {
                settings = defaultSettings ?? new TSettings();
            }

            if (settings != null)
            {
                services.AddSingleton(settings);
            }
        }
        
        var result = services
            .ToService<TSettings>()
            .IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(TSettings)));
        return result!;
    }
    
    /// <summary>
    /// Add configuration files to the configuration builder.<br/>
    /// <i><b>Note</b>: filenames without the extensions</i>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="directoryPath"></param>
    /// <param name="files"></param>
    /// <returns></returns>
    private static IConfigurationBuilder ToJsonFiles(
        this IConfigurationBuilder builder,
        string directoryPath,
        params (string jsonFileName, bool optional, bool reloadOnChange)[] files)
    {
        foreach (var (jsonFileName, optional, reloadOnChange) in files)
        {
            var filePath = Path.Combine($"{directoryPath}", $"{jsonFileName}.json");
            filePath.IsFile(nameof(filePath));
            builder.AddJsonFile(filePath, optional, reloadOnChange);
        }

        return builder;
    }
}