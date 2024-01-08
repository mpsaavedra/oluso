using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Logger.Abstractions.Extensions;
using Oluso.Logger.Serilog.Extensions;
using Oluso.Logger.Settings;

namespace Oluso.Logger.Extensions;

/// <summary>
/// 
/// </summary>
public static class LoggerServiceCollection
{
    /// <summary>
    /// register Logger service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddLoggerService(this IServiceCollection services, 
        LoggerSettings settings)
    {
        if (settings?.Serilog != null)
        {
            services.AddSerilogService(settings.Serilog);
        }

        return services;
    }

    /// <summary>
    /// register Logger service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddLoggerService(this IServiceCollection services,
        Action<LoggerOptions> options)
    {
        var settings = options!.ToSerilogConfigureOrDefault();
        if (settings.IsSerilogEnabled)
        {
            services.AddSerilogService(settings.SerilogOptions);
        }
        return services;
    }
}