using Oluso.Logger.Abstractions;

namespace Oluso.Logger.Serilog.Settings;

/// <summary>
/// Serilog options 
/// </summary>
public class SerilogOptions
{
    /// <summary>
    /// returns the  <see cref="SerilogSettings"/> object
    /// </summary>
    public SerilogSettings SerilogSettings { get; private set; } = new();

    /// <summary>
    /// Configure the Console logger.
    /// </summary>
    /// <returns></returns>
    public SerilogOptions AddConsole(
        LoggerLevel minimumLevel = SerilogConsoleSettings.DefaultMinimumLevel,
        string outputTemplate = SerilogConsoleSettings.DefaultOutputTemplate)
    {
        SerilogSettings.Console = new()
        {
            Enabled = true,
            OutputTemplate = outputTemplate,
            MinimumLevel = minimumLevel.ToString()
        };
        return this;
    }

    /// <summary>
    /// Configure the File logger
    /// </summary>
    /// <returns></returns>
    public SerilogOptions AddFile(
        string filePath = SerilogFileSettings.DefaultFilePath,
        LoggerLevel minimumLevel = SerilogFileSettings.DefaultMinimumLevel,
        LoggerFileInterval fileInterval = SerilogFileSettings.DefaultLoggerInterval)
    {
        SerilogSettings.File = new()
        {
            Enabled = true,
            FilePath = filePath,
            MinimumLevel = minimumLevel.ToString(),
            Interval = fileInterval.ToString()
        };
        return this;
    }

    /// <summary>
    /// Configure the Seq logger
    /// </summary>
    /// <returns></returns>
    public SerilogOptions AddSeq(
        string serverUrl,
        LoggerLevel minimumLogLever = SerilogSeqSettings.DefaultMinimumLevel,
        string? apiKey = SerilogSeqSettings.DefaultApiKey,
        HttpMessageHandler? messageHandler = null)
    {
        SerilogSettings.Seq = new()
        {
            Enabled = true,
            ServerUrl = serverUrl,
            MinimumLevel = minimumLogLever.ToString(),
            MessageHandler = messageHandler
        };
        return this;
    }

    /// <summary>
    /// Configure the Email logger
    /// </summary>
    /// <returns></returns>
    public SerilogOptions AddEmail(
        string from,
        string to,
        string server = SerilogEmailSettings.DefaultServer,
        int port = SerilogEmailSettings.DefaultPort,
        string? username = null,
        string? password = null,
        bool enableSsl = SerilogEmailSettings.DefaultEnableSsl,
        bool htmlBody = SerilogEmailSettings.DefaultIsBodyHtml,
        string subject = SerilogEmailSettings.DefaultSubject,
        string outputTemplate = SerilogEmailSettings.DefaultOutputTemplate,
        LoggerLevel minimumLevel = SerilogEmailSettings.DefaultMinimumLevel)
    {
        SerilogSettings.Email = new()
        {
            Server = server,
            Port = port,
            From = from,
            To = to,
            UserName = username,
            Password = password,
            EnableSsl = enableSsl,
            IsBodyHtml = htmlBody,
            Subject = subject,
            OutputTemplate = outputTemplate,
            MinimumLevel = minimumLevel.ToString()
        };
        return this;
    }
}