using Oluso.Logger.Abstractions;
using Oluso.Logger.Serilog.Settings;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Oluso.Logger.Serilog.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.Http;
using Serilog.Sinks.Seq;
using Serilog.Sinks.SystemConsole;
using ILogger = Serilog.ILogger;

namespace Oluso.Logger.Serilog;

public class SerilogProvider : Abstractions.ILogger, ILoggerService
{
    private ILogger? _logger;
    
    /// <summary>
    /// returns a new <see cref="SerilogProvider"/> instance
    /// </summary>
    /// <param name="settings"></param>
    public SerilogProvider(SerilogSettings settings) =>
        Settings = settings;
    
    /// <summary>
    /// Serilog settings
    /// </summary>
    public SerilogSettings Settings { get; set; }
    
    /// <summary>
    /// trace a message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Abstractions.ILogger Trace(string message)
    {
        _logger?.Verbose(message);
        Close();
        return this;
    }

    /// <summary>
    /// trace a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Abstractions.ILogger Trace(string message, object data)
    {
        _logger?.Verbose(message + " => {Data}", data);
        Close();
        return this;
    }

    /// <summary>
    /// logs a debug message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Abstractions.ILogger Debug(string message)
    {
        _logger?.Debug(message);
        Close();
        return this;
    }

    /// <summary>
    /// logs a debug message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Abstractions.ILogger Debug(string message, object data)
    {
        _logger?.Debug(message + " => {Data}", data);
        Close();
        return this;
    }

    /// <summary>
    /// logs an info message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Abstractions.ILogger Info(string message)
    {
        _logger?.Information(message);
        Close();
        return this;
    }

    /// <summary>
    /// logs a message with some data
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Abstractions.ILogger Info(string message, object data)
    {
        _logger?.Information(message + " => {Data}", data);
        Close();
        return this;
    }

    /// <summary>
    /// log a message with custom message and template
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="templateMessage"></param>
    /// <param name="propertyValues"></param>
    /// <returns></returns>
    public Abstractions.ILogger Info(string eventName, string templateMessage, params object[] propertyValues)
    {
        var message = "{EventName} => " + templateMessage;
        var values = new object[] { eventName }.Concat(propertyValues).ToArray();
        _logger?.Information(message, values);
        Close();
        return this;
    }

    /// <summary>
    /// logs an error message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Abstractions.ILogger Error(string message)
    {
        _logger?.Error(message);
        Close();
        return this;
    }

    /// <summary>
    /// logs an error
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Abstractions.ILogger Error(string message, object data)
    {
        _logger?.Error(message + " => {Data}", data);
        Close();
        return this;
    }

    /// <summary>
    /// logs and error
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="templateMessage"></param>
    /// <param name="propertyValues"></param>
    /// <returns></returns>
    public Abstractions.ILogger Error(string eventName, string templateMessage, params object[] propertyValues)
    {
        var message = "{EventName} => " + templateMessage;
        var values = new object[] { eventName }.Concat(propertyValues).ToArray();
        _logger?.Error(message, values);
        Close();
        return this;
    }

    /// <summary>
    /// logs the exception data
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public Abstractions.ILogger Error(Exception exception)
    {
        _logger?.Error("Exception => {Exception}", exception);
        Close();
        return this;
    }

    /// <summary>
    /// logs an error message with the exception data
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    public Abstractions.ILogger Error(string message, Exception exception)
    {
        _logger?.Error(message + " => {Exception}", exception);
        Close();
        return this;
    }

    /// <summary>
    /// returns a ne Logger
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public Abstractions.ILogger CreateLogger(string title)
    {
        _logger = new LoggerConfiguration()
            .AddDefaultSettings(title)
            .AddConsole(Settings.Console)
            .AddFile(Settings.File)
            .AddSeq(Settings.Seq)
            .CreateLogger();
        return this;
    }

    /// <summary>
    /// returns  a new Logger
    /// </summary>
    /// <typeparam name="TTitle"></typeparam>
    /// <returns></returns>
    public Abstractions.ILogger CreateLogger<TTitle>() =>
        CreateLogger(typeof(TTitle).Name);
    
    private void Close() => Log.CloseAndFlush();
}