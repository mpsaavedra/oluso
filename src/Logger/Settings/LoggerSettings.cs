using Oluso.Logger.Serilog.Settings;

namespace Oluso.Logger.Settings;

/// <summary>
/// Logger settings
/// </summary>
public class LoggerSettings
{
    /// <summary>
    /// Serilog settings
    /// </summary>
    public SerilogSettings? Serilog { get; set; } = new();
}