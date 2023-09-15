using Microsoft.Extensions.DependencyInjection;

namespace Oluso.Configuration.Hosting;

/// <summary>
/// Configuration builder definition that connections to the <see cref="IServiceCollection"/>
/// services of the host application
/// </summary>
public interface IConfigurationServiceBuilder
{
    /// <summary>
    /// Provides access to the host application <see cref="IServiceCollection"/>
    /// </summary>
    IServiceCollection Services { get; }
}