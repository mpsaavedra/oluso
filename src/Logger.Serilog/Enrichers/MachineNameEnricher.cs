using Serilog.Core;
using Serilog.Events;

namespace Oluso.Logger.Serilog.Enrichers;

/// <summary>
/// Log output enricher that allows to use the machine name for a better debug or
/// logs
/// </summary>
public class MachineNameEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enricher property name
    /// </summary>
    public const string MachineNameProperty = "MachineName";
    
    private LogEventProperty? _lastValue;
    
    /// <summary>
    /// Apply enricher to the log entry
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var machineName = Environment.MachineName;
        var last = this._lastValue;
        if (last is null || (string)((ScalarValue)last.Value).Value != machineName)
        {
            this._lastValue = last = new LogEventProperty(MachineNameProperty, new ScalarValue(machineName));
            logEvent?.AddPropertyIfAbsent(last);
        }
    }
}