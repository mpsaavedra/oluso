using Microsoft.Extensions.Configuration;
using Oluso.Logger.Abstractions;
using Oluso.Logger.Abstractions.Extensions;
using Oluso.Logger.Serilog;
using Oluso.Logger.Serilog.Settings;
using Oluso.Logger.Settings;

namespace Oluso.Logger;

public static class Log
{
    public static ILoggerService UseSerilog(ConfigurationManager configuration)
    {
        var loggerSection = configuration.GetSection("Logger");
        var loggerSettings = loggerSection.Get<LoggerSettings>();
        return UseSerilog(loggerSettings.Serilog!);
    }

    public static ILoggerService UseSerilog(Action<SerilogOptions> options) =>
        UseSerilog(options!.ToSerilogConfigureOrDefault().SerilogSettings);
    
    public static ILoggerService UseSerilog(SerilogSettings settings) =>
        new SerilogProvider(settings);
}