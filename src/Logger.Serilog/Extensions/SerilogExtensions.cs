using System.Net;
using Oluso.Logger.Abstractions.Extensions;
using Oluso.Logger.Serilog.Settings;
using global::Serilog;
using global::Serilog.Context;
using global::Serilog.Debugging;
using Serilog.Events;
using Seri = global::Serilog;

namespace Oluso.Logger.Serilog.Extensions;


/// <summary>
/// Serilog Logger related extensions
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// set default settings
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static LoggerConfiguration AddDefaultSettings(this LoggerConfiguration loggerConfiguration, string title)
    {
        SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));
        LogContext.PushProperty("Title", title);
        
        return loggerConfiguration.IsNullOrEmptyThrow(nameof(loggerConfiguration))
            .Enrich.WithMachineName()
            .Enrich.WithProcess()
            .Enrich.WithThread()
            .Enrich.FromLogContext();
    }

    /// <summary>
    /// Add console logger
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static LoggerConfiguration AddConsole(this LoggerConfiguration loggerConfiguration,
        SerilogConsoleSettings? settings)
    {
        if (settings?.Enabled == true)
        {
            loggerConfiguration.IsNullOrEmptyThrow(nameof(loggerConfiguration))
                .WriteTo.Console(
                    theme: Seri.Sinks.SystemConsole.Themes.SystemConsoleTheme.Colored,
                    outputTemplate: settings.OutputTemplate!,
                    restrictedToMinimumLevel: settings.MinimumLevel.ToMinimumLevel());
        }
        return loggerConfiguration;
    }

    /// <summary>
    /// Add file logger
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static LoggerConfiguration AddFile(this LoggerConfiguration loggerConfiguration,
        SerilogFileSettings settings)
    {
        if (settings?.Enabled == true)
        {
            loggerConfiguration.IsNullOrEmptyThrow(nameof(loggerConfiguration))
                .WriteTo.File(
                    path: settings.FilePath!,
                    restrictedToMinimumLevel: settings.MinimumLevel.ToMinimumLevel(),
                    rollingInterval:settings.Interval.ToRollingInterval(),
                    formatter: new Seri.Formatting.Compact.CompactJsonFormatter(),
                    shared: true);
        }
        return loggerConfiguration;
    }

    /// <summary>
    /// Add Seq logger
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static LoggerConfiguration AddSeq(this LoggerConfiguration loggerConfiguration,
        SerilogSeqSettings settings)
    {
        if (settings?.Enabled == true)
        {
            loggerConfiguration.IsNullOrEmptyThrow(nameof(loggerConfiguration))
                .WriteTo.Seq(
                    serverUrl: settings.ServerUrl ?? "seq://localhost",
                    apiKey: settings.ApiKey,
                    messageHandler: settings.MessageHandler,
                    restrictedToMinimumLevel: settings.MinimumLevel.ToMinimumLevel());
        }
        return loggerConfiguration;
    }

    /// <summary>
    /// add Email logger
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static LoggerConfiguration AddEmail(this LoggerConfiguration loggerConfiguration,
        SerilogEmailSettings settings)
    {
        if (settings?.Enabled == true)
        {
            loggerConfiguration.IsNullOrEmptyThrow(nameof(loggerConfiguration))
                .WriteTo.Email(
                    fromEmail: settings.Server ?? "localhost",
                    toEmail: settings.To,
                    mailServer: settings.Server,
                    mailSubject: settings.Subject,
                    restrictedToMinimumLevel: settings.MinimumLevel.ToMinimumLevel(),
                    outputTemplate: settings.OutputTemplate);
            settings.UserName.IsNullOrEmptyThrow(nameof(settings.UserName));
            settings.Password.IsNullOrEmptyThrow(nameof(settings.Password));
            settings.Server.IsNullOrEmptyThrow(nameof(settings.Server));
            settings.From.IsNullOrEmptyThrow(nameof(settings.From));
            settings.To.IsNullOrEmptyThrow(nameof(settings.To));

            loggerConfiguration.IsNullOrEmptyThrow(nameof(loggerConfiguration)).WriteTo.Email(
                new Seri.Sinks.Email.EmailConnectionInfo
                {
                    NetworkCredentials = new NetworkCredential
                    {
                        UserName = settings.UserName,
                        Password = settings.Password,
                    },
                    MailServer = settings.Server,
                    Port = settings.Port!.Value,
                    EnableSsl = settings.EnableSsl!.Value,
                    FromEmail = settings.From,
                    ToEmail = settings.To,
                    EmailSubject = settings.Subject,
                },
                outputTemplate: settings.OutputTemplate,
                restrictedToMinimumLevel: settings.MinimumLevel.ToMinimumLevel());
        }
        return loggerConfiguration;
    }

    /// <summary>
    /// cast from Abstractions.LoggerLevel loggerLevel to <see cref="Seri.Events.LogEventLevel"/>
    /// </summary>
    /// <param name="loggerLevel"></param>
    /// <returns></returns>
    public static Seri.Events.LogEventLevel ToMinimumLevel(this Abstractions.LoggerLevel loggerLevel) =>
        loggerLevel.ToString().ToMinimumLevel();

    /// <summary>
    /// cast from string to a valid <see cref="Seri.Events.LogEventLevel"/>
    /// </summary>
    /// <param name="loggerLevel"></param>
    /// <param name="defaultLevel"></param>
    /// <returns></returns>
    public static Seri.Events.LogEventLevel ToMinimumLevel(this string? loggerLevel,
        Seri.Events.LogEventLevel defaultLevel = Seri.Events.LogEventLevel.Information) =>
        loggerLevel switch
        {
            "Trace" => Seri.Events.LogEventLevel.Verbose,
            "Debug" => Seri.Events.LogEventLevel.Debug,
            "Info" => Seri.Events.LogEventLevel.Information,
            "Error" => Seri.Events.LogEventLevel.Error,
            "Fatal" => Seri.Events.LogEventLevel.Fatal,
            _ => defaultLevel,
        };
    
    /// <summary>
    /// cast from string to a valid <see cref="RollingInterval"/> value
    /// </summary>
    /// <param name="loggerInterval"></param>
    /// <param name="defaultInterval"></param>
    /// <returns></returns>
    public static RollingInterval ToRollingInterval(this string? loggerInterval,
        RollingInterval defaultInterval = RollingInterval.Day) =>
        loggerInterval switch
        {
            "Infinite" => RollingInterval.Infinite,
            "Year" => RollingInterval.Year,
            "Month" => RollingInterval.Month,
            "Hour" => RollingInterval.Hour,
            "Minute" => RollingInterval.Hour,
            _ => defaultInterval
        };
}