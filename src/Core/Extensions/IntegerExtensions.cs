#nullable enable
using System;
using System.Globalization;

namespace Oluso.Extensions;

/// <summary>
/// Integer related extensions
/// </summary>
public static class IntegerExtensions
{
    /// <summary>
    /// cast int to string
    /// </summary>
    /// <param name="input"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public static string ToStringFormat(this int input, IFormatProvider? formatProvider = null) => 
        input.ToString(formatProvider ?? CultureInfo.CurrentCulture);
}