using System;

namespace Oluso.Extensions;

/// <summary>
/// DateTime extensions
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// returns true if is weekend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool ToIsWeekend(this DateTime input)
        => input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday;
    
    /// <summary>
    /// return the age
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int ToAge(this DateTime input)
    {
        if (DateTime.Today.Month < input.Month
            || (DateTime.Today.Month == input.Month && DateTime.Today.Day < input.Day))
        {
            return DateTime.Today.Year - input.Year - 1;
        }
        else
        {
            return DateTime.Today.Year - input.Year;
        }
    }
    
    /// <summary>
    /// returns true if is the last day of the month
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool ToIsLastDayOfTheMonth(this DateTime input)
        => input == new DateTime(input.Year, input.Month, 1).AddMonths(1).AddDays(-1);
    
    /// <summary>
    /// returns the last day of the month
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static DateTime ToEndOfTheMonth(this DateTime input)
        => new DateTime(input.Year, input.Month, 1).AddMonths(1).AddDays(-1);
    
    /// <summary>
    /// returns the start of the week date
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static DateTime ToStartOfWeek(this DateTime input)
    {
        int dayOfWeek = (int)input.DayOfWeek;

        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;

        return input.AddDays(1 - dayOfWeek);
    }
    
    /// <summary>
    /// return the yesterday day from current day
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static DateTime ToYesterday(this DateTime input)
        => input.AddDays(-1);
    
    /// <summary>
    /// returns the date of tomorrow from current date
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static DateTime ToTomorrow(this DateTime input)
        => input.AddDays(1);
    
    /// <summary>
    /// Adds provided time string to the date. string time must be in the format: HH:mm:ss
    /// </summary>
    /// <param name="input"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime ToSetTime(this DateTime input, string time)
    {
        var parts = time.IsNullOrEmptyThrow(nameof(time)).Split(':');

        if (parts.Length < 3)
        {
            return input;
        }

        if (!int.TryParse(parts[0], out var hours))
        {
            return input;
        }

        if (!int.TryParse(parts[1], out var minutes))
        {
            return input;
        }

        if (!int.TryParse(parts[2], out var seconds))
        {
            return input;
        }

        return ToSetTime(input, hours, minutes, seconds);
    }
    
    /// <summary>
    /// add provided hours, minutes and seconds to the input DateTime.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static DateTime ToSetTime(this DateTime input, int hours, int minutes, int seconds)
    {
        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59 || seconds < 0 || seconds > 59)
        {
            return input;
        }

        return input.Date + new TimeSpan(hours, minutes, seconds);
    }
}