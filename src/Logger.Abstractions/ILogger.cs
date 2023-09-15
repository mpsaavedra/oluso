namespace Oluso.Logger.Abstractions;

/// <summary>
/// Logging contracts and basic functionalities
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Log anything, lowest level
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    ILogger Trace(string message);

    /// <summary>
    /// log anything, lowest level
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data">detail to display</param>
    /// <returns></returns>
    ILogger Trace(string message, object data);

    /// <summary>
    /// log internal messages when executing in debug mode
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    ILogger Debug(string message);

    /// <summary>
    /// log internal messages hen executing in debug mode
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    ILogger Debug(string message, object data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    ILogger Info(string message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    ILogger Info(string message, object data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="templateMessage"></param>
    /// <param name="propertyValues"></param>
    /// <returns></returns>
    ILogger Info(string eventName, string templateMessage, params object[] propertyValues);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
        "CA1716:Identifiers should not match with keywords", Justification = "<pendent>")]
    ILogger Error(string message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
        "CA1716:Identifiers should not match with keywords", Justification = "<pendent>")]
    ILogger Error(string message, object data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="templateMessage"></param>
    /// <param name="propertyValues"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
        "CA1716:Identifiers should not match with keywords", Justification = "<pendent>")]
    ILogger Error(string eventName, string templateMessage, params object[] propertyValues);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
        "CA1716:Identifiers should not match with keywords", Justification = "<pendent>")]
    ILogger Error(Exception exception);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
        "CA1716:Identifiers should not match with keywords", Justification = "<pendent>")]
    ILogger Error(string message, Exception exception);
}