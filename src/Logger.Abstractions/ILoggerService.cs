namespace Oluso.Logger.Abstractions;

/// <summary>
/// contract to create logger
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// create a new logger
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    ILogger CreateLogger(string title);

    /// <summary>
    /// create a new logger
    /// </summary>
    /// <typeparam name="TTitle"></typeparam>
    /// <returns></returns>
    ILogger CreateLogger<TTitle>();
}