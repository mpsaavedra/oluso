using Serilog.Core;
using Serilog.Events;

namespace Oluso.Logger.Serilog.Enrichers;

/// <summary>
/// Log output enricher that allows to add process data for a better debug or
/// logs
/// </summary>
public class ProcessEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enricher property name
    /// </summary>
    public const string ProcessPropertyName = "Process";
    
    private LogEventProperty? _lastValue;

    /// <summary>
    /// Apply enricher to log entry
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();
        string processValue = string.Empty;

        if (process?.Id > 0)
        {
            processValue = process.Id.ToString();
        }

        if (!string.IsNullOrWhiteSpace(process?.ProcessName))
        {
            processValue += process?.Id > 0 ? $" - {process.ProcessName}" : process?.ProcessName;
        }

        var last = this._lastValue;

        if (last is null || (string)((ScalarValue)last.Value).Value != processValue)
        {
            this._lastValue = last = new LogEventProperty(ProcessPropertyName, new ScalarValue(processValue));

            logEvent?.AddPropertyIfAbsent(last);
        }
    }
}