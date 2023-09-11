using System;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="ConsoleColor"/> extensions
/// </summary>
public static class ConsoleColorExtensions
{
    /// <summary>
    /// Writes a message into the console of a provided foreground color
    /// </summary>
    /// <param name="foregroundColor"></param>
    /// <param name="message"></param>
    public static void Write(this ConsoleColor foregroundColor, string message)
    {
        try
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(message);
            Console.ForegroundColor = oldColor;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <summary>
    /// Writes a message into the console of a provided foreground color
    /// </summary>
    /// <param name="foregroundColor"></param>
    /// <param name="message"></param>
    public static void WriteLine(this ConsoleColor foregroundColor, string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(message);
        Console.ForegroundColor = oldColor;
    }
}