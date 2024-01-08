using Microsoft.Extensions.Configuration;
using Oluso.Logger.Abstractions;
using Oluso.Logger.Abstractions.Extensions;
using Oluso.Logger.Serilog;
using Oluso.Logger.Serilog.Settings;
using Oluso.Logger.Settings;

namespace Oluso.Logger;

/// <summary>
/// Log related extensions
/// </summary>
public static class Log
{
    /// <summary>
    /// Register a serilog provider
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static ILoggerService UseSerilog(ConfigurationManager configuration)
    {
        var loggerSection = configuration.GetSection("Logger");
        var loggerSettings = loggerSection.Get<LoggerSettings>();
        return UseSerilog(loggerSettings.Serilog!);
    }

    /// <summary>
    /// Register a serilog provider
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ILoggerService UseSerilog(Action<SerilogOptions> options) =>
        UseSerilog(options!.ToSerilogConfigureOrDefault().SerilogSettings);
    
    /// <summary>
    /// Register a serilog provider
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static ILoggerService UseSerilog(SerilogSettings settings) =>
        new SerilogProvider(settings);
}