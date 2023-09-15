using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;

namespace Oluso.Configuration.Hosting;

/// <inheritdoc/>
public class ConfigurationService : IConfigurationService
{
    private readonly ILogger<ConfigurationService> _logger;
    private readonly IProvider _provider;
    private readonly IPublisher _publisher;

#pragma warning disable CS8625
#pragma warning disable CS8618
    /// <summary>
    /// returns a new <see cref="ConfigurationService"/> instance
    /// </summary>
    public ConfigurationService(ILogger<ConfigurationService> logger,
        IProvider provider, IPublisher publisher)
    {
#pragma warning restore CS8625
#pragma warning restore CS8618
        _logger = logger;
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
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
        }
        var paths = await _provider!.ListPaths();
        await PublishChanges(paths);
        await _provider.Watch(OnChange, cancellationToken);
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
            await _publisher.Publish(path, hash);
        }
    }

    /// <inheritdoc/>
    public async Task OnChange(IEnumerable<string> paths)
    {
        _logger.LogDebug("changes in configuration files detected, publishing using {Name}", _provider.Name);
        paths = paths.ToList();
        if (paths.Any())
        {
            await PublishChanges(paths);
        }
    }
}