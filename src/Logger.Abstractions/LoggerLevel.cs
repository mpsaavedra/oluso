namespace Oluso.Logger.Abstractions;

/// <summary>
/// loggers level
/// </summary>
public enum LoggerLevel
{
    /// <summary>
    /// log anything
    /// </summary>
    Trace = 0,

    /// <summary>
    /// log in debug mode
    /// </summary>
    Debug = 1,

    /// <summary>
    /// log system events
    /// </summary>
    Info = 2,

    /// <summary>
    /// log errors
    /// </summary>
    Error = 3,
}