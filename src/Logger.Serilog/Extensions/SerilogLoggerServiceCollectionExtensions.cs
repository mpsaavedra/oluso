using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oluso.Logger.Abstractions;
using Oluso.Logger.Abstractions.Extensions;
using Oluso.Logger.Serilog.Settings;

namespace Oluso.Logger.Serilog.Extensions;

/// <summary>
/// Serilog logger service collection related extensions
/// </summary>
public static class SerilogLoggerServiceCollectionExtensions
{
    /// <summary>
    /// Register the serilog service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"><see cref="SerilogSettings"/></param>
    /// <returns></returns>
    public static ILoggerService? AddSerilogService(this IServiceCollection services,
        SerilogSettings settings)
    {
        services.TryAddSingleton<ILoggerService>(new SerilogProvider(settings));
        return services.BuildServiceProvider().GetService<ILoggerService>();
    }

    /// <summary>
    /// Register the serilog service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ILoggerService? AddSerilogService(this IServiceCollection services,
        Action<SerilogOptions> options) =>
        services.AddSerilogService(options!.ToSerilogConfigureOrDefault().SerilogSettings);
}