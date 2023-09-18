using Oluso.Logger.Serilog.Settings;

namespace Oluso.Logger.Settings;

/// <summary>
/// Logger options to use as a configuration Action
/// </summary>
public class LoggerOptions
{
    /// <summary>
    /// true if serilog logger is enabled
    /// </summary>
    public bool IsSerilogEnabled {get; private set; }

    /// <summary>
    /// Serilog configuration action
    /// </summary>
    public Action<SerilogOptions> SerilogOptions { get; set; } = x => { };

    /// <summary>
    /// Set to ue Serilog Logger
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public LoggerOptions UseSerilog(Action<SerilogOptions> options)
    {
        SerilogOptions = options;
        IsSerilogEnabled = true;
        return this;
    }
}