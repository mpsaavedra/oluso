using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Oluso.Configuration.Client;

/// <summary>
/// Loggin utility class
/// </summary>
public static class Logger
{
#pragma warning disable CS8618
    private static ILoggerFactory _factory;
#pragma warning restore CS8618

    /// <summary>
    /// Get/Set <see cref="ILoggerFactory"/> instance, is not
    /// created it creates one.
    /// </summary>
    public static ILoggerFactory LoggerFactory
    {
        get => _factory ?? new NullLoggerFactory();
        set => _factory = value;
    }

    /// <summary>
    /// create a new Logger of type T.
    /// </summary>
    public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
}