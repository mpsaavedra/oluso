using Serilog.Core;
using Serilog.Events;

namespace Oluso.Logger.Serilog.Enrichers;

/// <summary>
/// Log output enricher that allows to add thread information for a better debug or
/// logs
/// </summary>
public class ThreadEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enricher property name
    /// </summary>
    public const string ThreadPropertyName = "Thread";
    
    private LogEventProperty? _lastValue;
    
    /// <summary>
    /// Apply enricher to the log entry
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var thread = Thread.CurrentThread;

        string threadValue = string.Empty;

        if (thread?.ManagedThreadId > 0)
        {
            threadValue = thread.ManagedThreadId.ToString();
        }

        if (!string.IsNullOrWhiteSpace(thread?.Name))
        {
            threadValue += thread?.ManagedThreadId > 0 ? $" - {thread.Name}" : thread?.Name;
        }

        var last = this._lastValue;

        if (last is null || (string)((ScalarValue)last.Value).Value != threadValue)
        {
            this._lastValue = last = new LogEventProperty(ThreadPropertyName, new ScalarValue(threadValue));

            logEvent?.AddPropertyIfAbsent(last);
        }
    }
}