using System;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="TimeSpan"/> extensions
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// turns some time into an string
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string ToFormat(this TimeSpan time)
    {
        var days = (time.Days > 0) ? $"{time.Days}d" : string.Empty;
        var hours = (time.Hours > 0) ? $"{time.Hours}d" : string.Empty;
        var minutes = (time.Minutes > 0) ? $"{time.Minutes}d" : string.Empty;
        var seconds = (time.Seconds > 0) ? $"{time.Seconds}d" : string.Empty;

        return $"{days} - {hours}:{minutes}:{seconds}".Trim();
    }
}