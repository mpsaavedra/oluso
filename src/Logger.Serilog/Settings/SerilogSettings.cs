namespace Oluso.Logger.Serilog.Settings;

/// <summary>
/// Serilog settings
/// </summary>
public class SerilogSettings
{
    /// <summary>
    /// Console settings, check <see cref="SerilogConsoleSettings"/>
    /// </summary>
    public SerilogConsoleSettings Console { get; set; } = new();

    /// <summary>
    /// File settings, check <see cref="SerilogFileSettings"/>
    /// </summary>
    public SerilogFileSettings File { get; set; } = new();

    /// <summary>
    /// Email settings to send log results, check <see cref="SerilogEmailSettings"/>
    /// </summary>
    public SerilogEmailSettings Email { get; set; } = new();

    /// <summary>
    /// Seq connection settings, check <see cref="SerilogSeqSettings"/>
    /// </summary>
    public SerilogSeqSettings Seq { get; set; } = new();
}