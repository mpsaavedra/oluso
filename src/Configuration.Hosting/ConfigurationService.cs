#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Hosting.Providers.Filesystem;

namespace Oluso.Configuration.Hosting;

/// <inheritdoc/>
public class ConfigurationService : IConfigurationService
{
    private readonly ILogger<ConfigurationService> _logger;
    private readonly IProvider _provider;
#pragma warning disable CS0649
    private readonly IPublisher _publisher;
#pragma warning restore CS0649

#pragma warning disable CS8625
#pragma warning disable CS8618
    /// <summary>
    /// returns a new <see cref="ConfigurationService"/> instance
    /// </summary>
    public ConfigurationService(ILogger<ConfigurationService> logger,
        IProvider provider, IPublisher? publisher = null)
    {
#pragma warning restore CS8625
#pragma warning restore CS8618
        _logger = logger;
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _logger!.LogDebug("Configuration Service has been created");
    }

    /// <inheritdoc/>
    public async Task Initialize(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("{Name} configuration service has been initialized and watching for changes", _provider.Name);
        _provider.Initialize();

        if (_provider != null)
        {
            _logger.LogDebug("Initializing publisher {Publisher}", nameof(_publisher));
            _publisher?.Initialize();
        }
        var paths = await _provider!.ListPaths();
        await PublishChanges(paths);
        await _provider.Watch(OnChange, cancellationToken);

        _logger.LogInformation("{Name} configuration watching for changes.", _provider.Name);
    }

    /// <inheritdoc/>
    public async Task PublishChanges(IEnumerable<string> paths)
    {
        if (_publisher == null)
        {
            return;
        }
        _logger.LogDebug("Publishing configuration changes ...");
        foreach (var path in paths)
        {
            var hash = await _provider.GetHash(path);
            var topic = path;
            if (_provider is FilesystemProvider provider)
            {
                if (!path.StartsWith(provider.Settings.Path))
                {
                    topic = path;
                }

                topic = path.Substring(provider.Settings.Path.Length + 1);
            }
            await _publisher?.Publish(topic, hash)!;
        }
    }

    /// <inheritdoc/>
    public async Task OnChange(IEnumerable<string> paths)
    {
        _logger.LogDebug("changes in configuration files detected, publishing using {Name}", _provider.Name);
        if (_provider is FilesystemProvider)
        {
            paths = paths
                .Select(x =>
                {
                    if(_provider is FilesystemProvider provider)
                    {
                        var settingsPath = provider?.Settings.Path;
                        var path = settingsPath != null &&
                                   !x.StartsWith(settingsPath)
                            ? Path.Combine(settingsPath, x)
                            : x;
                        path = path.EndsWith("~") ? path.Substring(0, path.Length - 1) : path;
                        return path;
                    }

                    return x;
                });
        }
        else
        {
            paths = paths.ToList();
        }
        if (paths.Any())
        {
            await PublishChanges(paths);
        }
    }
}