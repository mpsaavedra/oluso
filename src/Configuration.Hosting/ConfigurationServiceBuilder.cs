using System;
using Microsoft.Extensions.DependencyInjection;

namespace Oluso.Configuration.Hosting;

/// <inheritdoc/>
public class ConfigurationServiceBuilder : IConfigurationServiceBuilder
{
    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <summary>
    /// returns a new <see cref="ConfigurationService"/> instance
    /// </summary>
    public ConfigurationServiceBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}