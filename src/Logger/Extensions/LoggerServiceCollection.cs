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
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddLoggerService(IServiceCollection services, 
        IConfiguration configuration)
    {
        var settings =
            configuration?
                .GetSection(nameof(LoggerSettings))
                .Get<LoggerSettings>();

        if (settings?.SerilogSettings != null)
        {
            services.AddSerilogService(settings.SerilogSettings);
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