using Serilog.Core;
using Serilog.Events;

namespace Oluso.Logger.Serilog.Enrichers;

/// <summary>
/// Log output enricher that allows to add title for a better debug or
/// logs
/// </summary>
public class TitleEnricher :ILogEventEnricher
{
    /// <summary>
    /// Enricher property name
    /// </summary>
    public const string TitlePropertyName = "Title";
    
    private readonly string? _title;
    private LogEventProperty? _lastValue;

    /// <summary>
    /// returns a new <see cref="TitleEnricher"/> instance
    /// </summary>
    public TitleEnricher()
    {
        _title = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
    }

    /// <summary>
    /// returns a new <see cref="TitleEnricher"/> instance
    /// </summary>
    /// <param name="title"></param>
    public TitleEnricher(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            title = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        this._title = title;
    }
    
    /// <summary>
    /// Apply enricher to log entry
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var last = _lastValue;
        if (last is null || (string)((ScalarValue)last.Value).Value != this._title)
        {
            this._lastValue = last = new LogEventProperty(TitlePropertyName, new ScalarValue(_title));
            logEvent?.AddPropertyIfAbsent(last);
        }
    }
}